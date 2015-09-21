package perfit.statistic.analysis;

import java.util.List;

import perfit.statistic.data.Joint;
import perfit.statistic.data.Hand;
import perfit.statistic.data.Pushup;
import perfit.statistic.data.Shoulder;
import perfit.statistic.data.StandardHand;
import perfit.statistic.data.StandardJoint;

public interface Analysis {
	
	Double PUSHUP_STANDARD_ANGLE = 1.57d;
	Double PUSHUP_STANDARD_RECOVERY = 2d;
	Double PUSHUP_STANDARD = 3.5d;
	
	public StandardHand SetStandard(StandardHand shand, int j, double x, double y, double z);
	
	public Hand HandInit(Hand hand, int j, double x, double y, double z);
	
	public Hand HandInit(Hand hand, String score, Double time);
	
	public Hand HandInitF(Hand hand);
	
	public Joint JointAnalysis(Joint joint, StandardJoint sjoint);
	
	public List<Double> HandAnalysis(Hand hand, StandardHand shand);
	//return a list of the scores of 6 different fingers(include palm)
	//as order of palm-thumb-forefinger-middle finger-ring finger-little finger
	
	public Shoulder ShoulderInit(Shoulder shoulder, String score, Double time, Double angle);
	
	public List<Double> ShoulderAnalysis(Shoulder shoulder);
	
	public Pushup PushupInit(Pushup pushup, String score, Double time, Double angle, Double recovery);
	
	public List<Double> PushupAnalysis(Pushup pushup);
}
