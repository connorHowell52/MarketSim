package edu.georgiasouthern.finalproject;

import android.content.ContentValues;
import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

public class cash_outActivity extends AppCompatActivity {
    public TextView TimesWon;
    public TextView TimeLost;
    public TextView TimesRestart;
    public TextView MoneyWon;
    DatabaseHelper databaseHelper = new DatabaseHelper(this);
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.cash_out);
        TimesWon = findViewById(R.id.TimesWon);
        TimeLost = findViewById(R.id.TimesLost);
        TimesRestart = findViewById(R.id.TimesRestart);
        MoneyWon = findViewById(R.id.MoneyWon);
        int winNum = MainActivity.stats.getInt("TimesWon");
        int lossNum = MainActivity.stats.getInt("TimesLost");
        int moneyWon = MainActivity.stats.getInt("MoneyWon");
        boolean Bankrupt = MainActivity.stats.getBoolean("Bankrupt");
        int timesRestart = 0;
        if(Bankrupt){
            timesRestart = 1;
        }
        try{
            SQLiteDatabase data = databaseHelper.getReadableDatabase();
            Cursor cursor = data.rawQuery("select * from myTable where UserID=?",
                    new String[]{"Main"});
            if(cursor.moveToFirst()) {
                winNum = winNum + cursor.getInt(1);
                lossNum = lossNum +cursor.getInt(2);
                moneyWon= moneyWon+cursor.getInt(5);
                timesRestart = cursor.getInt(4)+timesRestart;
            }
            DatabaseHelper database = new DatabaseHelper(this);
            database.updateData("Main", winNum,lossNum,timesRestart,moneyWon);
            database.close();
        }catch(Exception e){
            SQLiteDatabase database = databaseHelper.getWritableDatabase();
            ContentValues contentValues = new ContentValues();
            contentValues.put("UserID", "Main");
            contentValues.put("TimesWon", winNum);
            contentValues.put("TimesLost", lossNum);
            contentValues.put("TimesRestart", timesRestart);
            contentValues.put("MoneyWon", moneyWon);
            database.insert("myTable", null, contentValues);
            database.close();
        }

        SQLiteDatabase data = databaseHelper.getReadableDatabase();
        Cursor cursor = data.rawQuery("select * from myTable where UserID=?",
                new String[]{"Main"});
        if(cursor.moveToFirst()){
            winNum = cursor.getInt(1);
            TimesWon.setText("Wins: "+winNum);
            lossNum = cursor.getInt(2);
            TimeLost.setText("Loses: "+lossNum);
            timesRestart = cursor.getInt(4);
            TimesRestart.setText("Times Bankrupt: "+timesRestart);
            moneyWon = cursor.getInt(5);
            MoneyWon.setText("Money Won: "+moneyWon);
        }
    }
    public void onRestart(View view){
        Intent startGame = new Intent(this, MainActivity.class);
        startActivity(startGame);
    }

}
