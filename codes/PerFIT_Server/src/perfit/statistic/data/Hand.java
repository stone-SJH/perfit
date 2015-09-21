package perfit.statistic.data;

import java.util.ArrayList;
import java.util.List;

import perfit.statistic.data.Joint;
public class Hand {
	
	public List<Joint> joints = new ArrayList<Joint>(22);
	
	//public Joint joint = new Joint();
	
	public int greats;
	public int goods;
	public int bads;
	
	public List<Double> times;

	public Hand(){
		//joints = new Joint[22];
		times = new ArrayList<Double>();
		for (int i = 0; i < 22; i++){
			Joint j = new Joint();
			if (i >= 0 && i <= 1)
				j.from = Joint.finger.palm;
			else if (i >= 2 && i <= 5)
				j.from = Joint.finger.thumb;
			else if (i >= 6 && i <= 9)
				j.from = Joint.finger.forefinger;
			else if (i >= 10 && i <= 13)
				j.from = Joint.finger.middlefinger;
			else if (i >= 14 && i <= 17)
				j.from = Joint.finger.ringfinger;
			else if (i >= 18 && i <= 21)
				j.from = Joint.finger.littlefinger;
			joints.add(j);
		}
	}
}
