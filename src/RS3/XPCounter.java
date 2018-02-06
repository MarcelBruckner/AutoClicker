package RS3;

import org.powerbot.script.rt6.ClientContext;

public class XPCounter {
    private ClientContext ctx;
    private int SKILLS[];

    private int startXP;
    private int[] startXPs;

    public XPCounter(ClientContext ctx, int[] skills) {
        this.ctx = ctx;
        this.SKILLS = skills;
        startXP = overallXP();
        startXPs = overallXPs();
    }

    private int overallXP(){
        int sum = 0;

        for(int i : SKILLS){
            sum += ctx.skills.experience(i);
        }

        return sum;
    }

    private int[] overallXPs(){
        int[] arr = new int[SKILLS.length];
        for (int i = 0; i < SKILLS.length; i++) {
            arr[i] = ctx.skills.experience(SKILLS[i]);
        }
        return arr;
    }

    private int[] gainedXPs(){
        int[] arr = new int[SKILLS.length];
        for (int i = 0; i < SKILLS.length; i++) {
            arr[i] = overallXPs()[i] - startXPs[i];
        }
        return arr;
    }

    public int gainedXP(){
        return overallXP() - startXP;
    }

    public int[] levelUpXP(){
        int[] arr = new int[SKILLS.length];
        for (int i = 0; i < SKILLS.length; i++) {
            arr[i] = ctx.skills.experienceAt(ctx.skills.level(SKILLS[i]) + 1);
        }
        return arr;
    }

    public int[] currentXP(){
        int[] arr = new int[SKILLS.length];
        for (int i = 0; i < SKILLS.length; i++) {
            arr[i] = ctx.skills.experience(SKILLS[i]);
        }
        return arr;
    }

    public int[] remainingXP(){
        int[] arr = new int[SKILLS.length];
        for (int i = 0; i < SKILLS.length; i++) {
            arr[i] = levelUpXP()[i] - currentXP()[i];
        }
        return arr;
    }
}
