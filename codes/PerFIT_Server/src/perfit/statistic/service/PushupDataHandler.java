package perfit.statistic.service;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.List;

import perfit.statistic.analysis.Analysis;
import perfit.statistic.analysis.AnalysisImp;
import perfit.statistic.data.Pushup;
import perfit.statistic.data.PushupHistory;

public class PushupDataHandler {
	private Analysis run = new AnalysisImp();
	private Pushup pushup = new Pushup();
	private PushupHistory his = new PushupHistory();
	
	private String[] dataSplit(String s){
		String[] result = s.split(" ");
		return result;
	}
	
	private void GetHistory(String fileName){
		File file = new File(fileName);  
        BufferedReader reader = null;
        if (file.exists()){
        try {  
            reader = new BufferedReader(new FileReader(file));  
            String tempString = null;
            List<Double> tmp = new ArrayList<Double> ();
            while ((tempString = reader.readLine()) != null){
            	tmp.add(Double.parseDouble(tempString));
            }
            his.total_amount = tmp.get(0);
            his.total_time = tmp.get(1);
            his.score1 = tmp.get(2);
            his.score2 = tmp.get(3);
            his.score3 = tmp.get(4);
            his.best_score1 = tmp.get(5);
            his.best_score2 = tmp.get(6);
            his.best_score3 = tmp.get(7);
            reader.close();
        }
        catch (IOException e) {  
            e.printStackTrace();  
        } finally {  
            if (reader != null) {  
                try {  
                    reader.close();  
                } catch (IOException e1) {  
                }  
            }  
        }  
        }
	}
	
	private void SetHistory(List<Double> result, String fileName){
		File file = new File(fileName);
		if(!file.exists())   
	    {   
	        try {   
	            file.createNewFile();   
	        } catch (IOException e) {    
	            e.printStackTrace();   
	        }   
	    }    
		try {    
				FileOutputStream fos = new FileOutputStream(fileName);  
				OutputStreamWriter osw = new OutputStreamWriter(fos);
				PrintWriter pw = new PrintWriter(osw);
				
				if (result.get(0) != 0)
					pw.print(his.total_amount + result.get(0) + "\r\n");
				else pw.print(his.total_amount + "\r\n");
				if (result.get(1) != 0)
					pw.print(his.total_time + result.get(1) + "\r\n");
				else pw.print(his.total_time + "\r\n");
				if (his.score1 != 0)
					pw.print((result.get(2)+his.score1)/2 + "\r\n");
				else pw.print(result.get(2) + "\r\n");
				if (his.score2 != 0)
					pw.print((result.get(3)+his.score2)/2 + "\r\n");
				else pw.print(result.get(3) + "\r\n");
				if (his.score3 != 0)
					pw.print((result.get(4)+his.score3)/2 + "\r\n");
				else pw.print(result.get(4) + "\r\n");
				
				if (result.get(2) > his.best_score1)
					pw.print(result.get(2) + "\r\n");
				else pw.print(his.best_score1 + "\r\n");
				
				if (result.get(3) > his.best_score2)
					pw.print(result.get(3) + "\r\n");
				else pw.print(his.best_score2 + "\r\n");
				
				if (result.get(4) > his.best_score3)
					pw.print(result.get(4) + "\r\n");
				else pw.print(his.best_score3 + "\r\n");
				
				pw.close();
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	private void PushupHandler(String fileName) {  
        File file = new File(fileName);  
        BufferedReader reader = null;
        try {  
            reader = new BufferedReader(new FileReader(file));  
            String tempString = null;  
            String[] temp;
            while ((tempString = reader.readLine()) != null) {  
                temp = dataSplit(tempString);
                double t = Double.parseDouble(temp[0]);
                double a = Double.parseDouble(temp[1]);
                double r = Double.parseDouble(temp[2]);
                run.PushupInit(pushup, temp[3], t, a, r);
            }  
            reader.close();  
        } catch (IOException e) {  
            e.printStackTrace();  
        } finally {  
            if (reader != null) {  
                try {  
                    reader.close();  
                } catch (IOException e1) {  
                }  
            }  
        }  
    }
	
	private String evaluate(List <Double> result, PushupHistory his)
	{
		String ret = "During the latest push-up exercise：";
		Double bend = result.get(2);
		Double unbend = result.get(3);
		Double allscore = result.get(4);
		if (bend > unbend)
		{
			ret += "your arm-bend movement performed better than your shoulder-recovery movement;";
		}
		else
		{
			ret += "your shoulder-recovery movement performed better than your arm-bend movement;";
		}
		if (allscore < 60)
		{
			ret += "you didn't match the standard, Please try harder next time!";
		}
		else if (allscore < 80)
		{
			ret += "you have matched the stardard, but still have a long way to go.";
		}
		else 
		{
			ret += "your performance was extremely excellent!";
		}
		if (bend > his.best_score1)
		{
			ret += "Your arm-bend movement scored the best ever.Congradulations!";
		}
		else if (bend > his.score1)
		{
			ret += "Your arm-bend movement scored better than the average before.";
		}
		if (unbend > his.best_score2)
		{
			ret += "Your shoulder-recovery movement scored the best ever.Congradulations!";
		}
		else if (unbend > his.score2)
		{
			ret += "Your shoulder-recovery movement scored better than the average before.";
		}
		if (allscore > his.best_score3)
		{
			ret += "Your whole push-up movement scored the best ever.Congradulations!";
		}
		else if (allscore > his.score3)
		{
			ret += "Your whole push-up movement scored better than the average before.";
		}
		return ret;
	}



	private void DataRelease(String fileName, String sfileName){
		File file = new File(fileName);
		if(!file.exists())   
	    {   
	        try {   
	            file.createNewFile();   
	        } catch (IOException e) {    
	            e.printStackTrace();   
	        }   
	    }    
		try {    
			
				List<Double> result = run.PushupAnalysis(pushup);
				GetHistory(sfileName);
				FileOutputStream fos = new FileOutputStream(fileName);  
				OutputStreamWriter osw = new OutputStreamWriter(fos);
				PrintWriter pw = new PrintWriter(osw);
				pw.print(pushup.greats + "\r\n");
				pw.print(pushup.goods + "\r\n");
				pw.print(pushup.bads + "\r\n");
				pw.print(result.get(0) + "\r\n");
				pw.print(result.get(1) + "\r\n");
				pw.print(result.get(2)+":"+his.score1 + "\r\n");
				pw.print(result.get(3)+":"+his.score2 + "\r\n");
				pw.print(result.get(4)+":"+his.score3 + "\r\n");
				pw.print(evaluate(result, his));
				pw.close();
				SetHistory(result, sfileName);
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	
	public void Handler(String datasource, String route, String hisroute){
		PushupHandler(datasource);
		DataRelease(route, hisroute);
	}
}
