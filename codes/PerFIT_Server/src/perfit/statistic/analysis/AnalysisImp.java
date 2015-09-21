package perfit.statistic.analysis;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import perfit.statistic.data.Hand;
import perfit.statistic.data.Joint;
import perfit.statistic.data.Pushup;
import perfit.statistic.data.Shoulder;
import perfit.statistic.data.StandardHand;
import perfit.statistic.data.StandardJoint;

public class AnalysisImp implements Analysis {

	@Override
	public StandardHand SetStandard(StandardHand shand, int j, double x,
			double y, double z) {
		// TODO Auto-generated method stub
		shand.joints.get(j).standardx = Math.abs(x);
		shand.joints.get(j).standardy = Math.abs(y);
		shand.joints.get(j).standardz = Math.abs(z);
		return shand;
	}

	@Override
	public Hand HandInit(Hand hand, int j, double x, double y, double z) {
		// TODO Auto-generated method stub
		hand.joints.get(j).Xac.add(Math.abs(x));
		hand.joints.get(j).Yac.add(Math.abs(y));
		hand.joints.get(j).Zac.add(Math.abs(z));
		hand.joints.get(j).count++;
		return hand;
	}
	
	@Override
	public Shoulder ShoulderInit(Shoulder shoulder, String score, Double time, Double angle){
		// TODO Auto-generated method stub
		if (score.equals("Great"))
			shoulder.greats++;
		else if (score.equals("Good"))
			shoulder.goods++;
		else if (score.equals("Bad"))
			shoulder.bads++;
		shoulder.times.add(time);
		shoulder.angles.add(angle);
		return shoulder;
	}
	
	public Pushup PushupInit(Pushup pushup, String score, Double time, Double angle, Double recovery){
		// TODO Auto-generated method stub
		if (score.equals("Great"))
			pushup.greats++;
		else if (score.equals("Good"))
			pushup.goods++;
		else if (score.equals("Bad"))
			pushup.bads++;
		pushup.times.add(time);
		pushup.angles.add(angle);
		pushup.recoverys.add(recovery);
		return pushup;
	}

	@Override
	public Hand HandInit(Hand hand, String score, Double time) {
		// TODO Auto-generated method stub
		if (score.equals("Great"))
			hand.greats++;
		else if (score.equals("Good"))
			hand.goods++;
		else if (score.equals("Bad"))
			hand.bads++;
		hand.times.add(time);
		return hand;
	}
	
	public Hand HandInitF(Hand hand){
		for (int j = 0; j < 22; j++){
			Joint joint = hand.joints.get(j);
			double xtot = 0, 
			       ytot = 0,
			       ztot = 0;
			for (int i = 0; i < joint.count; i++){
				xtot += joint.Xac.get(i);
				ytot += joint.Yac.get(i);
				ztot += joint.Zac.get(i);
			}
			joint.Xavg = xtot / joint.count;
			joint.Yavg = ytot / joint.count;
			joint.Zavg = ztot / joint.count;
		}
		return hand;
	}

	@Override
	public Joint JointAnalysis(Joint joint, StandardJoint sjoint) {
		// TODO Auto-generated method stub
		if (joint.count != 0){
			double xscore = Math.abs(joint.Xavg - sjoint.standardx);
			double yscore = Math.abs(joint.Yavg - sjoint.standardy);
			double zscore = Math.abs(joint.Zavg - sjoint.standardz);
			joint.score = 100 - (xscore + yscore*10 + zscore*0.6);
		}
		else joint.score = 0d;
		return joint;
	}
	
	private double GetScore(double s1, double s2){
		double result = 0;
		double count = 0;
		double total = 0;
		if (s1 != 0){
			count++;
			total += s1;
		}
		if (s2 != 0){
			count++;
			total += s2;
		}
		if (count != 0) result = total / count;
		if (result < 30) result = 30;
		return result;
	}
	
	private double GetScore(double s1, double s2, double s3, double s4){
		double result = 0;
		double count = 0;
		double total = 0;
		if (s1 != 0){
			count++;
			total += s1;
		}
		if (s2 != 0){
			count++;
			total += s2;
		}
		if (s3 != 0){
			count++;
			total += s3;
		}
		if (s4 != 0){
			count++;
			total += s4;
		}
		if (count != 0) result = total / count;
		if (result < 30) result = 30;
		return result;
	}

	@Override
	public List<Double> HandAnalysis(Hand hand, StandardHand shand) {
		// TODO Auto-generated method stub
		Double palmscore = 0d,
				thumbscore = 0d,
				forefingerscore = 0d,
				middlefingerscore = 0d,
				ringfingerscore = 0d,
				littlefingerscore = 0d;
		List<Double> result = new ArrayList<Double>();
		palmscore = GetScore(hand.joints.get(0).score, hand.joints.get(1).score);
		thumbscore = GetScore(hand.joints.get(2).score, hand.joints.get(3).score, hand.joints.get(4).score, hand.joints.get(5).score);
		forefingerscore = GetScore(hand.joints.get(6).score, hand.joints.get(7).score, hand.joints.get(8).score, hand.joints.get(9).score);
		middlefingerscore = GetScore(hand.joints.get(10).score, hand.joints.get(11).score, hand.joints.get(12).score, hand.joints.get(13).score);
		ringfingerscore = GetScore(hand.joints.get(14).score, hand.joints.get(15).score, hand.joints.get(16).score, hand.joints.get(17).score);
		littlefingerscore = GetScore(hand.joints.get(18).score, hand.joints.get(19).score, hand.joints.get(20).score, hand.joints.get(21).score);
		
		result.add(palmscore);
		result.add(thumbscore);
		result.add(forefingerscore);
		result.add(middlefingerscore);
		result.add(ringfingerscore);
		result.add(littlefingerscore);
		
		Double maxtime = 0d,
				mintime = 0d,
				avgtime = 0d;
		if (hand.times.size() != 0){
			maxtime = Collections.max(hand.times);
			mintime = Collections.min(hand.times);
			Double totaltime = 0d;
			for (int i = 0; i < hand.times.size(); i++){
				totaltime += hand.times.get(i);
			}
			avgtime = totaltime / hand.times.size();
		}
		result.add(maxtime);
		result.add(mintime);
		result.add(avgtime);
		return result;
	}
	
	@Override
	public List<Double> ShoulderAnalysis(Shoulder shoulder){
		List<Double> result = new ArrayList<Double>();
		
		Double maxtime = 0d,
				mintime = 0d,
				avgtime = 0d;
		if (shoulder.times.size() != 0){
			maxtime = Collections.max(shoulder.times);
			mintime = Collections.min(shoulder.times);
			Double totaltime = 0d;
			for (int i = 0; i < shoulder.times.size(); i++){
				totaltime += shoulder.times.get(i);
			}
			avgtime = totaltime / shoulder.times.size();
		}
		result.add(maxtime);
		result.add(mintime);
		result.add(avgtime);
		
		Double score1 = 0d,
				score2 = 0d,
				score3 = 0d;
		if (shoulder.angles.size() != 0){
			Double totalangles = 0d;
			Double avgangle = 0d;
			Double varangle = 0d;
			Double rangeangle = 0d;
			
			for (int i = 0; i < shoulder.angles.size(); i++){
				totalangles += shoulder.angles.get(i);
			}
			avgangle = totalangles / shoulder.angles.size();
			if (Math.abs(avgangle - 1) < 1){
				score1 = 100- Math.abs(avgangle-1)*100;
			}
			
			totalangles = 0d;
			for (int i = 0; i < shoulder.angles.size(); i++){
				totalangles += Math.pow(shoulder.angles.get(i)-avgangle, 2);
			}
			varangle = totalangles / shoulder.angles.size();
			score2 = 100 - varangle*100;
			if (score2 < 0) score2 = 0d;
			
			rangeangle = Collections.max(shoulder.angles) - Collections.min(shoulder.angles);
			score3 = 100 - rangeangle*50;
			if (score3 < 0) score3 = 0d;
		}
		result.add(score1);
		result.add(score2);
		result.add(score3);
		return result;
	}
	public List<Double> PushupAnalysis(Pushup pushup){
		List<Double> result = new ArrayList<Double>();
		int amount = pushup.times.size();
		result.add((double)amount);
		
		Double total_time = 0d;
		for(int i = 0; i < amount; i++){
			total_time += pushup.times.get(i);
		}
		result.add(total_time);
		
		Double total_angle = 0d,
				score1 = 0d,
				avg_angle = 0d;
		for (int i = 0; i < pushup.angles.size(); i++){
			total_angle += pushup.angles.get(i);
		}
		if (pushup.angles.size() != 0 && total_angle != 0){
			avg_angle = total_angle / pushup.angles.size();
			if (avg_angle <= PUSHUP_STANDARD_ANGLE)
				score1 = 100d;
			else 
				score1 = 100 - 80*(avg_angle - PUSHUP_STANDARD_ANGLE);
			if (score1 < 0) score1 = 0d;
		}
		result.add(score1);
		
		Double total_recovery = 0d,
				avg_recovery = 0d,
				score2 = 0d;
		for (int i = 0; i < pushup.recoverys.size(); i++){
			total_recovery += pushup.recoverys.get(i);
		}
		if (pushup.recoverys.size() != 0 && total_recovery != 0){
			avg_recovery = total_recovery / pushup.recoverys.size();
			if (avg_recovery <= PUSHUP_STANDARD_RECOVERY)
				score2 = 100d;
			else 
				score2 = 100 - 130*(avg_recovery - PUSHUP_STANDARD_RECOVERY);
			if (score2 < 0) score2 = 0d;
		}
		result.add(score2);
		
		Double avg = 0d,
				score3 = 0d;
		avg = avg_angle + avg_recovery;
		if (avg != 0){
			if (avg <= PUSHUP_STANDARD)
				score3 = 100d;
			else 
				score3 = 100 - 45*(avg - PUSHUP_STANDARD);
			if (score3 < 0) score3 = 0d;
		}
		result.add(score3);
		return result;
	}
}