using System;
using System.Collections.Generic;

public class Player {
    private int phase;
    private int faceDownCount;
    private int faceUpCount;
    private List<int> faceDown;
    private List<int> faceUp;
    private List<int> playersHand;

    public Player() {
        phase = 0;
        faceDownCount = 4;
        faceUpCount = 4;
        faceDown = new List<int>();
        faceUp = new List<int>();
        playersHand = new List<int>();
    }

    public void addCard(int card) {
        playersHand.Add(card);
    }

    public Boolean addFaceUp(int input) {
        Boolean found = false;

        for (int i = 0; i < playersHand.Count; i++) {
            if (playersHand[i] == input) {
                found = true;
                playersHand.RemoveAt(i);
                faceUp.Add(input);
                break;
            }
        }
        return found;
    }

    public void addFaceDown(int card) {
        faceDown.Add(card);
    }

    //TODO: this needs to be changed to an updateHand() function or something.. 
    //all this logic is for console display and kind of useless
    //converting the int value of the cards to characters does seem useful though
    /*
    public void displayHand() {
        System.out.print("Hand: ");
        for (int i = 0; i < playersHand.size(); i++) {
            if (playersHand.get(i) == 11) {
                System.out.print("[J] ");
            }
            else if (playersHand.get(i) == 12) {
                System.out.print("[Q] ");
            }
            else if (playersHand.get(i) == 13) {
                System.out.print("[K] ");
            }
            else if (playersHand.get(i) == 14) {
                System.out.print("[A] ");
            }
            else {
                System.out.print("[" + playersHand.get(i) + "] ");
            }
        }
        System.out.print("\n");
    }
    */

    // chooses the AI players face up cards based on their hand
    public void AIFaceUp() {
        int value = 14;
        int faceUpCount = 0;

        while (faceUpCount < 4) {
            for (int i = 0; i < playersHand.Count; i++) {
                if (playersHand[i] == 2 || playersHand[i] == 10) {
                    faceUp.Add(playersHand[i]);
                    playersHand.RemoveAt(i);
                    i--;
                    faceUpCount++;
                }
                else if (playersHand[i] == value) {
                    faceUp.Add(playersHand[i]);
                    playersHand.RemoveAt(i);
                    i--;
                    faceUpCount++;
                }
                 
                //needed in case AI hand contains more than 4 cards that are 2's, 10's, or A's. 
                //could rewrite this whole function to be slightly more efficient and make more sense but this logic will work
                if (faceUpCount > 3) {
                    i = playersHand.Count;
                }
            }
            value--;
        }
    }

    //player - multi-card play
    public Boolean play(int card, int frequency) {
        Boolean valid = false;

        if (phase == 2)
            valid = false;
        else if (phase == 1 && (this.getCardFrequencyFaceup(card) >= frequency))
            valid = true;
        else if (this.getCardFrequency(card) >= frequency)
            valid = true;
        else
            valid = false;
        return valid;
    }

    //player - 1 card play
    public Boolean play(int card) {
        Boolean valid = false;

        if ((phase == 1) && (this.getCardFrequencyFaceup(card) > 0))
            valid = true;
        else if (this.getCardFrequency(card) > 0)
            valid = true;
        else
            valid = false;
        return valid;
    }

    //chooses 'best' card for the AI to play based on situation
    public int AIplay(int lastCard) {
        int bestCard = 0;
        Boolean has2 = false;
        Boolean has10 = false;
        List<int> higherCards = new List<int>();
        if (this.phase == 1) {
            for (int i = 0; i < faceUpCount; i++) {
                if (faceUp[i] >= lastCard && faceUp[i] != 2 && faceUp[i] != 10) {
                    higherCards.Add(faceUp[i]);
                    bestCard = faceUp[i];
                }
                if (faceUp[i] == 2) {
                    has2 = true;
                }
                if (faceUp[i] == 10) {
                    has10 = true;
                }
            }

            for (int j = 0; j < higherCards.Count; j++) {
                if (higherCards[j] < bestCard) {
                    bestCard = higherCards[j];
                }
            }

            //if no moves but has a 10, play the 10
            if (bestCard == 0 && has10) {
                bestCard = 10;
            }//if no moves and no 10 but has a 2, play the 2
            else if (bestCard == 0 && has2) {
                bestCard = 2;
            }//otherwise no moves, pickup

        }
        else if (phase == 2) {
            bestCard = faceDown[0];
            if (bestCard < lastCard && bestCard != 10 && bestCard != 2) {
                bestCard = 0;
            }
        }
        else {
            for (int i = 0; i < playersHand.Count; i++) { //iterate thru hand
                if (playersHand[i] >= lastCard && playersHand[i] != 2 && playersHand[i] != 10) {
                    higherCards.Add(playersHand[i]);
                    bestCard = playersHand[i];
                }
                if (playersHand[i] == 2) {
                    has2 = true;
                }
                if (playersHand[i] == 10) {
                    has10 = true;
                }
            }

            for (int j = 0; j < higherCards.Count; j++) {
                if (higherCards[j] < bestCard) {
                    bestCard = higherCards[j];
                }
            }

            //if no moves but has a 10, play the 10
            if (bestCard == 0 && has10) {
                bestCard = 10;
            }//if no moves and no 10 but has a 2, play the 2
            else if (bestCard == 0 && has2) {
                bestCard = 2;
            }//otherwise no moves, pickup
        }
        return bestCard;
    }

    //method removes card from the correct location based on the phase
    public void removeCard(int card) {
        if (phase == 2) {
            faceDown.RemoveAt(card);
            faceDownCount--;
        }
        if (phase == 1) {
            for (int i = 0; i < faceUp.Count; i++) {
                if (faceUp[i] == card) {
                    faceUp.RemoveAt(i);
                    faceUpCount--;
                    break;
                }
            }
        }
        for (int i = 0; i < playersHand.Count; i++) {
            if (playersHand[i] == card) {
                playersHand.RemoveAt(i);
                break;
            }
        }
    }

    //returns the size of the player hand
    public int handSize() {
        return playersHand.Count;
    }

    //controls player phase
    public void setPhase(int phase) {
        this.phase = phase;
    }

    //returns number of face down cards left
    public int getFaceDownCount() {
        return this.faceDownCount;
    }

    //returns the number of face up cards left
    public int getFaceUpCount() {
        return this.faceUpCount;
    }

    //returns current player phase
    public int getPhase() {
        return this.phase;
    }

    //return value of face down card at given index
    public int getFaceDownCard(int index) {
        return faceDown[index];
    }

    //return frequency of input card in player hand
    public int getCardFrequency(int card) {
        int frequency = 0;

        for (int i = 0; i < playersHand.Count; i++) {
            if (playersHand[i] == card) {
                frequency++;
            }
        }
        return frequency;
    }

    //return frequency of input card in player faceup cards
    public int getCardFrequencyFaceup(int card) {
        int frequency = 0;

        for (int i = 0; i < faceUp.Count; i++) { //iterate thru hand
            if (faceUp[i] == card) {
                frequency++;
            }
        }
        return frequency;
    }

    //Method prints card data to the console. 
    //useless, maybe convert into an update() function? to update the player visuals when needed on the UI
    /*
    public void visualize(String user) {
        if (user == "Player") {
            System.out.println("** YOU **");
        }
        else if (user == "AI") {
            System.out.println("** CPU **");
        }
        else {
            System.out.println("** TEST **");
        }

        System.out.print("   Hand			 ");
        for (int i = 0; i < playersHand.size(); i++) {
            if (user == "AI") {
                System.out.print("[?] ");
            }
            else {
                if (playersHand.get(i) == 11) {
                    System.out.print("[J] ");
                }
                else if (playersHand.get(i) == 12) {
                    System.out.print("[Q] ");
                }
                else if (playersHand.get(i) == 13) {
                    System.out.print("[K] ");
                }
                else if (playersHand.get(i) == 14) {
                    System.out.print("[A] ");
                }
                else {
                    System.out.print("[" + playersHand.get(i) + "] ");
                }
            }
        }
        System.out.print("\n");

        System.out.print("   Face Up		 ");
        for (int i = 0; i < faceUp.size(); i++) {
            if (faceUp.get(i) == 11) {
                System.out.print("[J] ");
            }
            else if (faceUp.get(i) == 12) {
                System.out.print("[Q] ");
            }
            else if (faceUp.get(i) == 13) {
                System.out.print("[K] ");
            }
            else if (faceUp.get(i) == 14) {
                System.out.print("[A] ");
            }
            else {
                System.out.print("[" + faceUp.get(i) + "] ");
            }
        }
        System.out.print("\n");

        System.out.print("   Face Down		 ");
        for (int i = 0; i < faceDown.size(); i++) {
            if (user == "test") {
                if (faceDown.get(i) == 11) {
                    System.out.print("[J] ");
                }
                else if (faceDown.get(i) == 12) {
                    System.out.print("[Q] ");
                }
                else if (faceDown.get(i) == 13) {
                    System.out.print("[K] ");
                }
                else if (faceDown.get(i) == 14) {
                    System.out.print("[A] ");
                }
                else {
                    System.out.print("[" + faceDown.get(i) + "] ");
                }
            }
            else {
                System.out.print("[?] ");
            }
        }
        System.out.print("\n\n");
    }
    */
}