package perfit.statistic.data;

import java.util.ArrayList;
import java.util.List;

public class Shoulder {
	
	public int greats;
	public int goods;
	public int bads;
	
	public List<Double> times;
	public List<Double> angles;
	
	public Shoulder(){
		times = new ArrayList<Double>();
		angles = new ArrayList<Double>();
	}
}
