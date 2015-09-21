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
import perfit.statistic.data.Shoulder;
import perfit.statistic.data.ShoulderHistory;

public class ShoulderDataHandler {
	
	private Analysis run = new AnalysisImp();
	private Shoulder shoulder = new Shoulder();
	private ShoulderHistory his = new ShoulderHistory();
	
	private String[] dataSplit(String s){
		String[] result = s.split(" ");
		return result;
	}
	
	private void ShoulderHandler(String fileName) {  
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
                run.ShoulderInit(shoulder, temp[2], t, a);
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
            his.avgtime = tmp.get(3);
            his.score1 = tmp.get(0);
            his.score2 = tmp.get(1);
            his.score3 = tmp.get(2);
            his.best_score1 = tmp.get(4);
            his.best_score2 = tmp.get(5);
            his.best_score3 = tmp.get(6);
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
				if (his.score1 != 0)
					pw.print((result.get(3)+his.score1)/2 + "\r\n");
				else pw.print(result.get(3) + "\r\n");
				if (his.score2 != 0)
					pw.print((result.get(4)+his.score2)/2 + "\r\n");
				else pw.print(result.get(4) + "\r\n");
				if (his.score3 != 0)
					pw.print((result.get(5)+his.score3)/2 + "\r\n");
				else pw.print(result.get(5) + "\r\n");
				if (his.avgtime != 0)
					pw.print((result.get(2)+his.avgtime)/2 + "\r\n");
				else pw.print(result.get(2) + "\r\n");
				
				if (result.get(3) > his.best_score1)
					pw.print(result.get(3) + "\r\n");
				else pw.print(his.best_score1 + "\r\n");
				
				if (result.get(4) > his.best_score2)
					pw.print(result.get(4) + "\r\n");
				else pw.print(his.best_score2 + "\r\n");
				
				if (result.get(5) > his.best_score3)
					pw.print(result.get(5) + "\r\n");
				else pw.print(his.best_score3 + "\r\n");
				
				pw.close();
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	
	private String evaluate(List <Double> result, ShoulderHistory his)
	{
		String ret = "During the latest arm exercise:";
		Double comp = result.get(3);
		Double stable = result.get(4);
		Double wave = result.get(5);
		if (comp < 60)
		{
			ret += "Your completion score didn't match the standard. Please try harder next time!";
		}
		else if (comp < 80)
		{
			ret += "Your completion score has matched the standard, but you still have a long way to go.";
		}
		else 
		{
			ret += "Your completion score was completely a miracle! How did you accomplish this?";
		}
		
		if (stable < 60)
		{
			ret += "Your stability score didn't match the standard. Please try harder next time!您在稳定性方面表现不合格，请加油。";
		}
		else if (stable < 80)
		{
			ret += "Your stability score has matched the standard, but that's far from enough.";
		}
		else 
		{
			ret += "Your stability score was really azaming!";
		}
		
		if (wave < 60)
		{
			ret += "Your  score didn't match the standard. Please try harder next time!";
		}
		else if (wave < 80)
		{
			ret += "Your fluctuation score has matched the standard, you can do it better!";
		}
		else 
		{
			ret += "Nobody can be compared with you on fluctuaion score!";
		}
		if (comp > his.best_score1)
		{
			ret += "Your completion scored best ever. Congradulations!";
		}
		else if (comp > his.score1)
		{
			ret += "Your comoletion scored better than average before.";
		}
		if (stable > his.best_score2)
		{
			ret += "Your stability scored best ever. Congradulations!";
		}
		else if (stable > his.score2)
		{
			ret += "Your stability scored better than average before.";
		}
		if (wave > his.best_score3)
		{
			ret += "Your fluctuation scored best ever. Congradulations!";
		}
		else if (wave > his.score3)
		{
			ret += "Your fluctuation scored better than average before.";
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
			
				List<Double> result = run.ShoulderAnalysis(shoulder);
				GetHistory(sfileName);
				FileOutputStream fos = new FileOutputStream(fileName);  
				OutputStreamWriter osw = new OutputStreamWriter(fos);
				PrintWriter pw = new PrintWriter(osw);
				pw.print(shoulder.greats + "\r\n");
				pw.print(shoulder.goods + "\r\n");
				pw.print(shoulder.bads + "\r\n");
				pw.print(result.get(2)+":"+his.avgtime + "\r\n");
				pw.print(result.get(0) + "\r\n");
				pw.print(result.get(1) + "\r\n");
				pw.print(result.get(3)+":"+his.score1 + "\r\n");
				pw.print(result.get(4)+":"+his.score2 + "\r\n");
				pw.print(result.get(5)+":"+his.score3 + "\r\n");
				pw.print(evaluate(result, his));
				pw.close();
				SetHistory(result, sfileName);
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	
	public void Handler(String datasource, String route, String hisroute){
		ShoulderHandler(datasource);
		DataRelease(route, hisroute);
	}
}
