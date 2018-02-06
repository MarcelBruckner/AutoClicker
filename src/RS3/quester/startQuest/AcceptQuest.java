package RS3.quester.startQuest;

import RS3.Quester;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;

import java.util.concurrent.Callable;

public class AcceptQuest extends StartQuestTask {

    public AcceptQuest(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        System.out.println(ctx.widgets.component(acceptButton[0], acceptButton[1]).visible());
        return ctx.widgets.component(acceptButton[0], acceptButton[1]).visible();
    }

    @Override
    public void execute() {
        ctx.widgets.component(acceptButton[0], acceptButton[1]).click();
        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !activate();
            }
        }, 150, 20);
        Quester.progress = -1;
        while(ctx.chat.canContinue()) {
            ctx.chat.clickContinue();
            Condition.sleep(Random.nextInt(100, 200));
        }
    }
}
