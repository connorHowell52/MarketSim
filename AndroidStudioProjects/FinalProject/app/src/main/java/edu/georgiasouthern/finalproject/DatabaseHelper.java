package edu.georgiasouthern.finalproject;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteOpenHelper;
import android.database.sqlite.SQLiteDatabase;

public class DatabaseHelper extends SQLiteOpenHelper {
    public DatabaseHelper(Context context) {
        super(context, "mydb", null, 2);
    }
    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("create table myTable (UserID text primary key, TimesWon Integer, TimesLost Integer, TimesRestart Integer, MoneyWon Integer)");
    }
    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("drop table if exists myTable");
        onCreate(db);
    }
    public boolean updateData(String ID, int TimesWon, int TimesLost, int TimesRestart, int MoneyWon){
        SQLiteDatabase database = this.getWritableDatabase();
        ContentValues contentValues = new ContentValues();
        contentValues.put("UserID", ID);
        contentValues.put("TimesWon", TimesWon);
        contentValues.put("TimesLost", TimesLost);
        contentValues.put("TimesRestart", TimesRestart);
        contentValues.put("MoneyWon", MoneyWon);
        database.update("myTable", contentValues, "UserID = ?", new String[]{"Main"});
        return true;
    }

}