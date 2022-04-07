package edu.georgiasouthern.finalproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.Random;

public class MainActivity extends AppCompatActivity {
    ArrayList<String> cards = new ArrayList<>();
    LinearLayout layout;
    int cardsDealt;
    int cardsDealtDealer;
    int userScore;
    int dealerScore;
    TextView userScoreText;
    TextView dealerScoreText;
    ArrayList<String> userCards = new ArrayList<String>();
    ArrayList<String> dealerCards = new ArrayList<>();
    static TextView Result;
    int bet_num = 0;
    int money_left = 0;
    TextView money;
    boolean startgame = false;
    EditText bet;
    String tempCardName2;
    String tempCardName1;
    int wins = 0;
    int loses = 0;
    public static Bundle stats;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        layout = findViewById(R.id.cardLayout1);
        userScoreText = findViewById(R.id.userScore);
        Result = findViewById(R.id.Result);
        money = findViewById(R.id.Money_left);
        money_left = Integer.parseInt(money.getText().toString());
        dealerScoreText= findViewById(R.id.DealerScore);
        stats = new Bundle();
    }
    public void evaluateScore(View view){
        if(dealerScore>21||(userScore>dealerScore&&userScore<=21)){
            Result.setText("You Win!");
            money_left += bet_num;
            wins+=1;
            money.setText(money_left+"");
        }else if(dealerScore == userScore) {
            Result.setText("Push!");
        }else{
            Result.setText("You Lose!");
            loses+=1;
            money_left -= bet_num;
            money.setText(money_left+"");
        }
    }
    public void onStay(View view) throws InterruptedException {
        if(startgame == true) {
            evaluateAce(userCards, userScore);
            flipOver(view);
            startgame = false;
            evaluateAce(dealerCards,dealerScore);
            while ((dealerScore < 17)) {
                dealCardsDealer(cards.get(0));
                cards.remove(0);
                evaluateAce(dealerCards, dealerScore);
            }
            evaluateScore(view);
        }
    }
    public void flipOver(View view){
        layout = findViewById(R.id.dealerLayout1);
        layout.removeAllViews();
        int resID = getResources().getIdentifier(tempCardName1, "drawable", getPackageName());
        ImageView card = new ImageView(MainActivity.this);
        card.setImageResource(resID);
        addAView(card, 200, 180);
        dealerScoreText.setText("Dealer Score: " + dealerScore + "/21");
        resID = getResources().getIdentifier(tempCardName2, "drawable", getPackageName());
        card = new ImageView(MainActivity.this);
        card.setImageResource(resID);
        addAView(card, 200, 180);
        dealerScoreText.setText("Dealer Score: " + dealerScore + "/21");
    }
    public void onDeal(View view) {
        if (startgame == false) {
            bet = findViewById(R.id.bet_num);
            bet_num = Integer.parseInt(bet.getText().toString());
            cardsDealt = 0;
            cardsDealtDealer = 0;
            userScore = 0;
            dealerScore = 0;
            if (bet_num > money_left || money_left <= 0) {
            } else {
                Result.setText("");
                for (int i = 0; i < userCards.size(); i++) {
                    userCards.remove(0);
                }
                for (int i = 0; i < dealerCards.size(); i++) {
                    dealerCards.remove(0);
                }
                layout = findViewById(R.id.cardLayout1);
                layout.removeAllViews();
                layout = findViewById(R.id.cardLayout2);
                layout.removeAllViews();
                layout = findViewById(R.id.dealerLayout1);
                layout.removeAllViews();
                layout = findViewById(R.id.dealerLayout2);
                layout.removeAllViews();
                startgame = true;
                defualtCards(cards);
                shufCards(cards);
                dealCards(cards.get(0));
                cards.remove(0);
                dealCardsDealer(cards.get(0));
                cards.remove(0);
                dealCards(cards.get(0));
                cards.remove(0);
                dealCardsDealer(cards.get(0));
                cards.remove(0);

            }
        }
    }
    public void onhit(View view){
        if(userScore<21&&startgame == true) {
            dealCards(cards.get(0));
            cards.remove(0);
            if(userScore>21){
                flipOver(view);
                Result.setText("Busted!");
                money_left -= bet_num;
                money.setText(money_left+"");
                loses += 1;
                startgame = false;
            }
        }
    }
    public void dealCardsDealer(String cardToDeal){
        if(cardsDealtDealer >4){
            layout = findViewById(R.id.dealerLayout2);
        }else{
            layout = findViewById(R.id.dealerLayout1);
        }
        if(cardsDealtDealer == 1) {
            tempCardName2 = cardToDeal;
            cardToDeal = "backdesign_1";
            int resID = getResources().getIdentifier(cardToDeal, "drawable", getPackageName());
            ImageView card = new ImageView(MainActivity.this);
            card.setImageResource(resID);
            addAView(card, 200, 180);
            int score_to_add = getCardScore(tempCardName2);
            dealerCards.add(tempCardName2);
            dealerScore += score_to_add;
            cardsDealtDealer += 1;
        }else {
            if (cardsDealtDealer == 0) {
                tempCardName1 = cardToDeal;
            }
            int resID = getResources().getIdentifier(cardToDeal, "drawable", getPackageName());
            ImageView card = new ImageView(MainActivity.this);
            card.setImageResource(resID);
            addAView(card, 200, 180);
            int score_to_add = getCardScore(cardToDeal);
            dealerCards.add(cardToDeal);
            dealerScore += score_to_add;
            dealerScoreText.setText("Dealer Score: " + dealerScore + "/21");
            cardsDealtDealer += 1;
        }
    }
    public void dealCards(String cardToDeal){
        if(cardsDealt>4){
            layout = findViewById(R.id.cardLayout2);
        }else{
            layout = findViewById(R.id.cardLayout1);
        }
        int resID = getResources().getIdentifier(cardToDeal, "drawable", getPackageName());
        ImageView card = new ImageView(MainActivity.this);
        card.setImageResource(resID);
        addAView(card, 200, 180);
        int score_to_add = getCardScore(cardToDeal);
        userCards.add(cardToDeal);
        userScore += score_to_add;
        userScoreText.setText("Your Score: "+userScore+"/21");
        cardsDealt += 1;
    }
    public void evaluateAce(ArrayList<String> cardsDealt, int score){
        if(score<=11){
            for(int i = 0; i<cardsDealt.size(); i++){
                if(cardsDealt.get(i).contains("1")&&userScore<=11&&startgame==true){
                    userScore += 10;
                    userScoreText.setText("Your Score: "+userScore+"/21");
                    startgame = false;
                }else if((cardsDealt.get(i).contains("1")&&dealerScore<=11&&(dealerScore+10>userScore))||cardsDealt.get(i).contains("1")&&((dealerScore+10)==21)){
                    for(int x = 0; x<dealerCards.size(); x++){
                        dealerCards.remove(x);
                    }
                    dealerScore += 10;
                    dealerScoreText.setText("Your Score: "+dealerScore+"/21");
                }
            }
        }
    }
    public void addAView(ImageView imageView, int width, int height){
        LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(width,height);
        layoutParams.setMargins(0,10,0,10);
        imageView.setLayoutParams(layoutParams);
        layout.addView(imageView);
    }
    public static void shufCards(ArrayList<String> cards){
        ArrayList<String> temp = new ArrayList<>(cards);
        int size;
        cards.removeAll(temp);
        for(int i = 0; i<52; i++) {
            Random rand = new Random();
            size = temp.size();
            int x = rand.nextInt(size);
            String toadd = temp.get(x);
            cards.add(toadd);
            temp.remove(x);
        }
    }
    public static int getCardScore(String string){
        for (int i=1; i<10; i++){
            if(string.contains(i+"_")){
                return i;
            }
        }
        return 10;
    }
    public static void defualtCards(ArrayList<String> cards) {
        for (int i = 0; i < cards.size(); i++) {
            cards.remove(0);
        }
        for (int i = 1; i <= 13; i++) {
            String toadd = "";
            if (i <= 10) {
                toadd = "a" + i + "_";
            } else if (i == 11) {
                toadd = "jack_";
            } else if (i == 12) {
                toadd = "king_";
            } else if (i == 13) {
                toadd = "queen_";
            }
            for (int x = 0; x < 4; x++) {
                String temp = toadd;
                if (x == 0) {
                    toadd = toadd + "clubs";
                } else if (x == 1) {
                    toadd = toadd + "diamonds";
                } else if (x == 2) {
                    toadd = toadd + "spades";
                } else if (x == 3) {
                toadd = toadd + "hearts";
                }
            cards.add(toadd);
            toadd = temp;
        }
        }
    }
    public void onCashOut(View view) {
        if (startgame == false) {
            MainActivity.stats.putInt("TimesWon", wins);
            MainActivity.stats.putInt("TimesLost", loses);
            MainActivity.stats.putInt("MoneyWon", (money_left - 500));
            if (money_left == 0) {
                MainActivity.stats.putBoolean("Bankrupt", true);
            }
            Intent endGame = new Intent(this, cash_outActivity.class);
            startActivity(endGame);
        }
    }

}

