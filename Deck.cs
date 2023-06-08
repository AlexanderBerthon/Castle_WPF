using System;
using System.Collections.Generic;

public class Deck{
	public Random rn = new Random();
	private List<int> deck = new List<int>();

	public Deck() {
		int value = 2;
		int count = 0;

		for (int i = 0; i < 52; i++) {
			deck.Add(value);
			count++;
			if (count == 4) {
				value++;
				count = 0;
			}
		}
	}

	public int drawCard() {
		int randomIndex = rn.Next(Math.Abs(deck.Count));
		int drawnCard = deck[randomIndex];
		deck.RemoveAt(randomIndex);

		if(deck.Count == 0 ) {
			//print system message ("last card drawn from deck")
		}

		return drawnCard;
	}

	public int cardsLeft() {
		return deck.Count;
	}

}
