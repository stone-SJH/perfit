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
import perfit.statistic.data.Hand;
import perfit.statistic.data.HandHistory;
import perfit.statistic.data.StandardHand;

public class DataHandler {
    private Analysis run = new AnalysisImp();

    private StandardHand sleft = new StandardHand();
    private StandardHand sright = new StandardHand();
    private Hand left = new Hand();
    private Hand right = new Hand();
    private HandHistory lhis = new HandHistory();
    private HandHistory rhis = new HandHistory();
    
	
	private String[] dataSplit(String s){
		String[] result = s.split(" ");
		return result;
	}
	
	private void GetHistory(HandHistory his, String fileName){
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
            his.pscore = tmp.get(0);
            his.tscore = tmp.get(1);
            his.fscore = tmp.get(2);
            his.mscore = tmp.get(3);
            his.rscore = tmp.get(4);
            his.lscore = tmp.get(5);
            his.avgtime = tmp.get(6);
            his.best_pscore = tmp.get(7);
            his.best_tscore = tmp.get(8);
            his.best_fscore = tmp.get(9);
            his.best_mscore = tmp.get(10);
            his.best_rscore = tmp.get(11);
            his.best_lscore = tmp.get(12);
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
	
	private void SetHistory(HandHistory his, List<Double> result, String fileName){
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
				if (his.pscore != 0)
					pw.print((result.get(0)+his.pscore)/2 + "\r\n");
				else pw.print(result.get(0) + "\r\n");
				if (his.tscore != 0)
					pw.print((result.get(1)+his.tscore)/2 + "\r\n");
				else pw.print(result.get(1) + "\r\n");
				if (his.fscore != 0)
					pw.print((result.get(2)+his.fscore)/2 + "\r\n");
				else pw.print(result.get(2) + "\r\n");
				if (his.mscore != 0)
					pw.print((result.get(3)+his.mscore)/2 + "\r\n");
				else pw.print(result.get(3)+ "\r\n");
				if (his.rscore != 0)
					pw.print((result.get(4)+his.rscore)/2 + "\r\n");
				else pw.print(result.get(4) + "\r\n");
				if (his.lscore != 0)
					pw.print((result.get(5)+his.lscore)/2 + "\r\n");
				else pw.print(result.get(5) + "\r\n");
				if (his.avgtime != 0)
					pw.print((result.get(8)+his.avgtime)/2 + "\r\n");
				else pw.print(result.get(8) + "\r\n");
				
				if (result.get(0) > his.best_pscore)
					pw.print(result.get(0) + "\r\n");
				else pw.print(his.best_pscore + "\r\n");
				
				if (result.get(1) > his.best_tscore)
					pw.print(result.get(1) + "\r\n");
				else pw.print(his.best_tscore + "\r\n");
				
				if (result.get(2) > his.best_fscore)
					pw.print(result.get(2) + "\r\n");
				else pw.print(his.best_fscore + "\r\n");
				
				if (result.get(3) > his.best_mscore)
					pw.print(result.get(3) + "\r\n");
				else pw.print(his.best_mscore + "\r\n");
				
				if (result.get(4) > his.best_rscore)
					pw.print(result.get(4) + "\r\n");
				else pw.print(his.best_rscore + "\r\n");
				
				if (result.get(5) > his.best_lscore)
					pw.print(result.get(5) + "\r\n");
				else pw.print(his.best_lscore + "\r\n");
				pw.close();
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	private void HandHandler(String fileName) {  
        File file = new File(fileName);  
        BufferedReader reader = null;
        try {  
            reader = new BufferedReader(new FileReader(file));  
            String tempString = null;  
            String[] temp;
            while ((tempString = reader.readLine()) != null) {  
                temp = dataSplit(tempString);
                if (temp.length == 5){
                	int h = Integer.parseInt(temp[0]);
                	int j = Integer.parseInt(temp[1]);
                	double x = Double.parseDouble(temp[2]);
                	double y = Double.parseDouble(temp[3]);
                	double z = Double.parseDouble(temp[4]);
                	if (h == 0)
                		run.HandInit(left, j, x, y, z);
                	else 
                		run.HandInit(right, j, x, y, z);
                }
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
	
	private void HandScoreHandler(String fileName) {  
        File file = new File(fileName);  
        BufferedReader reader = null;
        try {  
            reader = new BufferedReader(new FileReader(file));  
            String tempString = null;  
            String[] temp;
            while ((tempString = reader.readLine()) != null) {  
                temp = dataSplit(tempString);
                int h = Integer.parseInt(temp[0]);
                double t = Double.parseDouble(temp[2]);
                if (h == 0)
                	run.HandInit(left, temp[1], t);
                else 
                	run.HandInit(right, temp[1], t);
                	
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
	private String evaluate(List <Double> result, char lr, HandHistory his)
	{
		String ret = "";
		if (lr == 'l')
		{
			ret = "During the latest left-hand exercise:";
		}
		else
		{
			ret = "During the latest right-hand exercise:";
		}
		Double best, worst;
		String be, wo;
		best = worst = result.get(1);
		be = wo = Order2String(1);
		for (int i = 2; i < 6; i++)
		{
			if (result.get(i) > best)
			{
				best = result.get(i);
				be = Order2String(i);
			}
			if (result.get(i) < worst)
			{
				best = result.get(i);
				wo = Order2String(i);
			}
		}
		ret += "your " + be + " performed the best;";
		ret += "your "+ wo + " performed the worst;";
		List <String> fail = new ArrayList<String>();
		for (int i = 1; i < 6; i++)
		{
			if (result.get(i) < 70)
			{
				fail.add(Order2String(i));
			}
		}
		
		if (fail.size() > 0)
		{
			for (int i = 0; i < fail.size(); i++)
			{
				if (i != fail.size() - 1)
				{
					ret += fail.get(i) + "、";				
				}
				else
				{
					ret += fail.get(i) + "didn't match the standard(70 percent),Please try harder next time!";
				}
			}
		}
		else 
			ret += "All your fingers performed well enough to match the standard";
		List <String> succ = new ArrayList<String>();
		if (result.get(1) > his.best_tscore)
		{
			succ.add(Order2String(1));
		}
		if (result.get(2) > his.best_fscore)
		{
			succ.add(Order2String(2));
		}
		if (result.get(3) > his.best_mscore)
		{
			succ.add(Order2String(3));
		}
		if (result.get(4) > his.best_rscore)
		{
			succ.add(Order2String(4));
		}
		if (result.get(5) > his.best_lscore)
		{
			succ.add(Order2String(5));
		}
		if (succ.size() > 0)
		{
			for (int i = 0; i < succ.size(); i++)
			{
				if (i != succ.size() - 1)
				{
					ret += succ.get(i) + "、";				
				}
				else
				{
					ret += succ.get(i) + "have accomplished a new record!Congradulations!";
				}
			}
		}
		
		return ret;
	}

	private String Order2String(int i) 
	{
		if (i == 0)
		{
			return "palm";
		}
		if (i == 1)
		{
			return "thumb";
		}
		if (i == 2)
		{
			return "forefinger";
		}
		if (i == 3)
		{
			return "middlefinger";
		}
		if (i == 4)
		{
			return "ringfinger";
		}
		if (i == 5)
		{
			return "littlefinger";
		}
		return "";
	}
	

	
	private void StandardHandHandler(){
		run.HandInitF(left);
		run.HandInitF(right);
		Double ana1 = Math.abs(left.joints.get(11).Zavg - left.joints.get(12).Zavg);
		Double ana2 = Math.abs(left.joints.get(11).Yavg - left.joints.get(12).Yavg);
		Double ana3 = Math.abs(right.joints.get(11).Zavg - right.joints.get(12).Zavg);
		Double ana4 = Math.abs(right.joints.get(11).Yavg - right.joints.get(12).Yavg);
		Double left_attach = 1d;
		if (ana2 != 0) left_attach = Math.abs((ana1 / ana2) / 1.5);
		Double right_attach = 1d;
		if (ana4 != 0) right_attach = Math.abs((ana3 / ana4) / 1.5);
		for (int i = 0; i < 22; i++){
			run.SetStandard(sleft, i, left.joints.get(i).Xavg*left_attach, left.joints.get(i).Yavg*left_attach, left.joints.get(i).Zavg*left_attach);
			run.SetStandard(sright, i, right.joints.get(i).Xavg*right_attach, right.joints.get(i).Yavg*right_attach, right.joints.get(i).Zavg*right_attach);
		}
	}
	
	
	private void DataRelease(String fileName1, String fileName2, String lfileName, String rfileName){
		File file = new File(fileName1);
		File file2 = new File(fileName2);
		if(!file.exists())   
	    {   
	        try {   
	            file.createNewFile();   
	        } catch (IOException e) {    
	            e.printStackTrace();    
	        }   
	    }    
		if (!file2.exists())
		{
			try{
				file.createNewFile();
			} catch (IOException e){
				e.printStackTrace();
			}
		}
		try {    
				for(int i = 0; i < 22; i++){
					run.JointAnalysis(left.joints.get(i), sleft.joints.get(i));
					run.JointAnalysis(right.joints.get(i), sright.joints.get(i));
				}
				List<Double> lresult = run.HandAnalysis(left, sleft);
				List<Double> rresult = run.HandAnalysis(right, sright);
				
				GetHistory(lhis, lfileName);
				GetHistory(rhis, rfileName);

				FileOutputStream fos1 = new FileOutputStream(fileName1);  
				OutputStreamWriter osw1 = new OutputStreamWriter(fos1);
				FileOutputStream fos2 = new FileOutputStream(fileName2);
				OutputStreamWriter osw2 = new OutputStreamWriter(fos2);
				PrintWriter pw = new PrintWriter(osw1);
				PrintWriter pw2 = new PrintWriter(osw2);
				pw.print(lresult.get(0)+":"+lhis.pscore + "\r\n");
				pw.print(lresult.get(1)+":"+lhis.tscore + "\r\n");
				pw.print(lresult.get(2)+":"+lhis.fscore + "\r\n");
				pw.print(lresult.get(3)+":"+lhis.mscore + "\r\n");
				pw.print(lresult.get(4)+":"+lhis.rscore + "\r\n");
				pw.print(lresult.get(5)+":"+lhis.lscore + "\r\n");
				pw.print(left.greats + "\r\n");
				pw.print(left.goods + "\r\n");
				pw.print(left.bads + "\r\n");
				pw.print(lresult.get(8)+":"+lhis.avgtime + "\r\n");
				pw.print(lresult.get(6) + "\r\n");
				pw.print(lresult.get(7) + "\r\n");
				pw2.print(rresult.get(0)+":"+rhis.pscore + "\r\n");
				pw2.print(rresult.get(1)+":"+rhis.tscore + "\r\n");
				pw2.print(rresult.get(2)+":"+rhis.fscore + "\r\n");
				pw2.print(rresult.get(3)+":"+rhis.mscore + "\r\n");
				pw2.print(rresult.get(4)+":"+rhis.rscore + "\r\n");
				pw2.print(rresult.get(5)+":"+rhis.lscore + "\r\n");
				pw2.print(right.greats + "\r\n");
				pw2.print(right.goods + "\r\n");
				pw2.print(right.bads + "\r\n");
				pw2.print(rresult.get(8)+":"+rhis.avgtime + "\r\n");
				pw2.print(rresult.get(6) + "\r\n");
				pw2.print(rresult.get(7) + "\r\n");
				pw.print(evaluate(lresult, 'l', lhis));
				pw2.print(evaluate(rresult, 'r', rhis));
				pw.close();
				pw2.close();
				SetHistory(lhis, lresult, lfileName);
				SetHistory(rhis, rresult, rfileName);
	        } catch (IOException e) {  
	            e.printStackTrace();  
	        }  
	}
	
	public void Handler(String datasource, String scoresource, String route, String route2, String leftroute, String rightroute){
		HandHandler(datasource);
		StandardHandHandler();
		HandScoreHandler(scoresource);
		DataRelease(route, route2, leftroute, rightroute);
	}
}
