package perfit.statistic.data;

import java.util.ArrayList;
import java.util.List;

public class Pushup {
	
	public int greats;
	public int goods;
	public int bads;
	
	public List<Double> times;
	public List<Double> angles;
	public List<Double> recoverys;
	
	public Pushup(){
		times = new ArrayList<Double>();
		angles = new ArrayList<Double>();
		recoverys = new ArrayList<Double>();
	}
}
