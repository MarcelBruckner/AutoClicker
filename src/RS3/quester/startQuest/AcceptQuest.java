package RS3.quester.startQuest;

import org.powerbot.script.Condition;
import org.powerbot.script.rt6.ClientContext;

import java.util.concurrent.Callable;

public class AcceptQuest extends StartQuestTask {

    public AcceptQuest(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return acceptButton.visible();
    }

    @Override
    public void execute() {
        acceptButton.click();
        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return !activate();
            }
        }, 150, 20);
    }
}
