using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : Controller<MenuController>
{
	public GameObject startMenu;
	public GameObject menu;
	public GameObject win;
	public GameObject titles;

	private float titlesSpeed = 4.2f;

	public bool isMovement = false;

	public void ForceStart()
	{
		AdventureController.main.ForceStart();
		startMenu.SetActive(false);
	}

	public void ForceExit()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void StartEndGame()
	{
		win.SetActive(true);
	}

	void Update()
	{
		if (!startMenu.activeInHierarchy)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				menu.SetActive(!menu.activeInHierarchy);
			}
		}

	}

	private void FixedUpdate()
	{
		if (titles.activeInHierarchy)
		{
			titles.transform.localPosition += new Vector3(0f, titlesSpeed,0f);

			if (titles.transform.localPosition.y > 3000)
				ForceExit();
		}
	}
}
