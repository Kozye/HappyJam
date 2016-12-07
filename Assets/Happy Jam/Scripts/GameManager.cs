using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;

public class GameManager : MonoSingleton<GameManager>
{
	[Header ("Read-Only")]
	[SerializeField]
	private int _coinsCollected = 0;
	[SerializeField]
	private int _coinScore = 0;

	public int CoinScore
	{
		get
		{
			return _coinScore;
		}

		set
		{
			_coinScore += value;
			_coinsCollected++;
		}
	}
}
