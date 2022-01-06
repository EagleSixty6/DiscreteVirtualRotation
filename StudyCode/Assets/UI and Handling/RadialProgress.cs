using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
	public Image LoadingBar;
	public float currentValue;
	public float speed; //degrees per second

	// Use this for initialization
	void Start()
	{
		speed = 0;
		currentValue = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (currentValue < 100)
		{
			currentValue += speed * Time.deltaTime;
		}

		LoadingBar.fillAmount = currentValue / 100;
	}
}