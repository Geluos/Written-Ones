using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : Controller<MenuController>
{
	public GameObject startMenu;
	public GameObject menu;
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
}
