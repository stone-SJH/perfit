package perfit.statistic.data;

import java.util.ArrayList;
import java.util.List;

public class Joint {
	public enum finger{
		thumb,
		forefinger,
		middlefinger,
		ringfinger,
		littlefinger,
		palm
	};
	
	public finger from;
	
	public List<Double> Xac;
	public List<Double> Yac;
	public List<Double> Zac;
	
	public int count;
	public Double score;
	
	public Double Xavg;
	public Double Yavg;
	public Double Zavg;
	
	public Joint(){
		Xac = new ArrayList<Double>();
		Yac = new ArrayList<Double>();
		Zac = new ArrayList<Double>();
		count = 0;
		score = 0d;
		Xavg = 0d;
		Yavg = 0d;
		Zavg = 0d;
	}
}
