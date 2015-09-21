package perfit.statistic.data;

import java.util.ArrayList;
import java.util.List;

public class StandardHand {
	
	public List<StandardJoint> joints = new ArrayList<StandardJoint>();

	public StandardHand(){
		for (int i = 0; i < 22; i++){
			StandardJoint sj = new StandardJoint();
			if (i >= 0 && i <= 1)
				sj.from = Joint.finger.palm;
			else if (i >= 2 && i <= 5)
				sj.from = Joint.finger.thumb;
			else if (i >= 6 && i <= 9)
				sj.from = Joint.finger.forefinger;
			else if (i >= 10 && i <= 13)
				sj.from = Joint.finger.middlefinger;
			else if (i >= 14 && i <= 17)
				sj.from = Joint.finger.ringfinger;
			else if (i >= 18 && i <= 21)
				sj.from = Joint.finger.littlefinger;
			joints.add(sj);
		}
	}
}
