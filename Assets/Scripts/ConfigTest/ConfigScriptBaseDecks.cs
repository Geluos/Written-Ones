using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Card;
using Unity.VisualScripting;
using static UnityEngine.UI.GridLayoutGroup;
using TreeEditor;
using System.Linq;
using UnityEditor;
using System.IO;
public static class CSVLogger
{
    // Путь к файлу (в папке StreamingAssets)
    private static string _filePath = Path.Combine(Application.streamingAssetsPath, "data.csv");

    /// <summary>
    /// Добавляет строку с двумя double значениями в CSV-файл.
    /// </summary>
    /// <param name="value1">Первое значение</param>
    /// <param name="value2">Второе значение</param>
    public static void AddLineToCSV(List<double> values)
    {
        // Формируем строку для записи (разделитель - запятая)
        string line = "";

        foreach (double value in values)
            line += value.ToString() + ';';

        // Проверяем, существует ли файл
        if (!File.Exists(_filePath))
        {
            // Создаём директорию, если её нет
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            // Записываем заголовок (опционально)
            File.WriteAllText(_filePath, "Value1,Value2\n");
        }

        // Добавляем строку в файл
        File.AppendAllText(_filePath, line + "\n");
    }
}

public class ConfigScriptBaseDecks : ConfigurationTest
{
    public Deck additionCards;
    public int popCount = 50;
    public int generations = 8;

    public override void Test()
    {
        //base.Test();
        genetic_algo();
    }



    static double grade(TestConfig con, ref double sanc, bool log = false)
    {
        double grade = 0.0;
        double san = 0.0;
        foreach (var item in con.resultsOfConfiguration.results)
        {
            if (item.isWin)
                grade += 1.0;
        }

        if (log)
        {
            Debug.Log("WinPercent = " + grade);
        }

        if (grade > 85.0)
        {
            san += grade - 85;
        }

        foreach (var hero in con.heroesSet)
        {
            if (hero.startDeck.cards.Count < 3 || hero.startDeck.cards.Count > 5)
                san += 2 * Math.Min(Math.Abs(hero.startDeck.cards.Count - 3), Math.Abs(hero.startDeck.cards.Count - 5));

            foreach (var card in hero.startDeck.cards)
                if (card.rarity != Card.Rarity.Common)
                {
                    san += 3.0;
                }
        }

        grade -= san;

        if (log)
        {
            Debug.Log("Best grade is " + grade);
        }

        sanc = san;
        return grade;
    }

    

    public struct Gen
    {
        public TestConfig conf;
        public double grade;
        public double sanc;

        public Gen(TestConfig conf) : this()
        {
            this.conf = conf;
            grade = double.NaN;
        }

        public void GradeThis()
        {
            grade = grade(conf, ref sanc);
        }
    }

    class GenComparer : IComparer<Gen>
    {
        public int Compare(Gen x, Gen y)
        {
            return x.grade.CompareTo(y.grade);
        }
    }

    //private TestConfig crossover(TestConfig con1, TestConfig con2)
    //{
    //    var con = con1.Copy();
    //    var rnd = UnityEngine.Random.value;
    //    int i = 0;
    //    foreach (var hero in con2.heroesSet)
    //    {
    //        if (rnd <= 0.5)
    //        {
    //            con.heroesSet[i] = Instantiate(hero);
    //        }
    //        ++i;
    //    }

    //    rnd = UnityEngine.Random.value;

    //    if (rnd <= 0.2)
    //    {
    //        mutate(con);
    //        mutate(con);
    //    }
    //    else if (rnd <= 0.5)
    //    {
    //        mutate(con);
    //    }

    //    return con;
    //}

    private TestConfig crossover(TestConfig con1, TestConfig con2)
    {
        var con = con1.Copy();
        var rnd = UnityEngine.Random.value;
        foreach (var hero in con.heroesSet)
        {
            hero.startDeck.cards = new List<Card>();
        }

        for (int i = 0; i < con.heroesSet.Count; ++i)
        {
            foreach(var card in con1.heroesSet[i].startDeck.cards)
            {
                rnd = UnityEngine.Random.value;
                if (rnd <= 0.5)
                {
                    con.heroesSet[i].startDeck.cards.Add(card.copy());
                }
            }
        }

        rnd = UnityEngine.Random.value;

        if (rnd <= 0.1)
        {
            mutate(con);
            mutate(con);
        }
        else if (rnd <= 0.3)
        {
            mutate(con);
        }

        return con;
    }


    private void mutate(TestConfig con)
    {
        var rnd = UnityEngine.Random.value;
        rnd *= 3;
        OwnerType owner;
        Deck deck;
        if(rnd < 1.0)
        {
            deck = con.heroesSet[0].startDeck;
            owner = con.heroesSet[0].ownerTypeForCharacter;
        }
        else if (rnd < 2.0)
        {
            deck = con.heroesSet[1].startDeck;
            owner = con.heroesSet[1].ownerTypeForCharacter;
        }
        else
        {
            deck = con.heroesSet[2].startDeck;
            owner = con.heroesSet[2].ownerTypeForCharacter;
        }

        rnd = UnityEngine.Random.value;
        List<Card> cardByOwner = new List<Card>();
        foreach (var card in additionCards.cards)
        {
            if (card.otype == owner || card.otype == OwnerType.Other)
            {
                cardByOwner.Add(card.copy());
            }
        }

        //Delete
        if (rnd < 0.3)
        {
            if (deck.cards.Count > 0)
            {
                int next = UnityEngine.Random.Range(0, deck.cards.Count);
                deck.cards.RemoveAt(next);
            }
        }
        //Add
        else if (rnd < 0.6)
        {
            int next = UnityEngine.Random.Range(0, cardByOwner.Count);
            deck.cards.Add(cardByOwner[next].copy());
        }
        //Replace
        else
        {
            if (deck.cards.Count > 0)
            {
                int next1 = UnityEngine.Random.Range(0, deck.cards.Count);
                int next2 = UnityEngine.Random.Range(0, cardByOwner.Count);
                deck.cards[next1] = cardByOwner[next2].copy();
            }
            else
            {
                int next = UnityEngine.Random.Range(0, cardByOwner.Count);
                deck.cards.Add(cardByOwner[next].copy());
            }
        }

    }

    Gen TournamentSelection(List<Gen> population, int tornamentSize)
    {
        List<Gen> list = new List<Gen>();
        for (int i = 0; i < tornamentSize; i++)
        {
            int parent = UnityEngine.Random.Range(0, population.Count);
            list.Add(population[parent]);
        }

        list.OrderByDescending(p => p.grade);

        return list.First();
    }


    void genetic_algo()
    {
        

        List<Gen> population = new List<Gen>();

        for (int i = 0; i < popCount; i++)
        {
            var gen = new Gen(config.Copy());

            int next = UnityEngine.Random.Range(1, 3);

            for (int j = 0; j < next; j++)
                mutate(gen.conf);

            gen.conf.play(100);
            gen.GradeThis();
            population.Add(gen);
        }

        Gen best = population[0];

        for (int i = 0; i < generations; i++)
        {
            List<Gen> newPopulation = new List<Gen>();

            //for (int j = 0; (j < population.Count); j++)
            //{
            //    if (population[j].grade == double.NaN)
            //    {
            //        population[j].conf.play();
            //        population[j].GradeThis();
            //    }


            //}

            for (int j = 0; j < population.Count - 4; j++)
            {
                var par1 = TournamentSelection(population, 3);
                var par2 = TournamentSelection(population, 3);

                var child = new Gen(crossover(par1.conf, par2.conf));

                child.conf.play(100);
                child.GradeThis();
                newPopulation.Add(child);
            }

            population = population.OrderByDescending(p => p.grade).Take(4).Concat(newPopulation).OrderByDescending(p => p.grade).ToList();

            //grade(population.First().conf, true);
            best = population.OrderByDescending(p => p.grade).First();
            CSVLogger.AddLineToCSV(new List<double> { best.grade, best.sanc , population.Average(x=> x.grade) });
            Debug.Log("End of generation");
        }


        AssetDatabase.CreateAsset(best.conf.heroesSet[0].startDeck, "Assets/TestConfig/Results/BestStartingDeck1.asset");
        AssetDatabase.CreateAsset(best.conf.heroesSet[1].startDeck, "Assets/TestConfig/Results/BestStartingDeck2.asset");
        AssetDatabase.CreateAsset(best.conf.heroesSet[2].startDeck, "Assets/TestConfig/Results/BestStartingDeck3.asset");

        AssetDatabase.CreateAsset(best.conf, "Assets/TestConfig/Results/BestStartingDecksFile.asset");


        //grade(best.conf, true);
        AssetDatabase.SaveAssets();

    }
}
