using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectMenuController : MonoBehaviour {
	[SerializeField] private InputField addressInput;
	[SerializeField] private NetworkManager netManager;

	private void Start() {
		netManager = FindObjectOfType<NetworkManager>();
	}

	public void Host() {
		LoadAddress();
		netManager.StartHost();
		HideMenu();
	}

	public void Join() {
		LoadAddress();
		netManager.StartClient();
		HideMenu();
	}

	private void LoadAddress() {
		netManager.networkAddress = addressInput.text;
	}

	private void HideMenu() {
		gameObject.SetActive(false);
	}
}
