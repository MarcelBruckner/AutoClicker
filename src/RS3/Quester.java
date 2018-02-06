package RS3;

import RS3.quester.CheckProgress;
import RS3.quester.EnterFirst.EnterFirst;
import RS3.quester.KillRanger.KillRanger;
import RS3.quester.WalkToXenia.GoToXenia;
import RS3.quester.enterCatacombs.EnterCatacombs;
import RS3.quester.startQuest.StartQuest;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.PollingScript;
import org.powerbot.script.Script;

import java.util.ArrayList;
import java.util.List;

@Script.Manifest(name = "Quester", description = "Does quests", properties = "client=6; author=Marcel; topic=999")

public class Quester extends PollingScript<ClientContext> {

    List<RS3Task> tasks = new ArrayList<RS3Task>();

    public static int progress = -1;

    @Override
    public void start() {
        tasks.add(new CheckProgress(ctx));
        tasks.add(new GoToXenia(ctx));
        tasks.add(new StartQuest(ctx));
        tasks.add(new EnterCatacombs(ctx));
        tasks.add(new EnterFirst(ctx));
        tasks.add(new KillRanger(ctx));
    }

    @Override
    public void poll() {
        System.out.println("progress = " + progress);
        for (RS3Task task : tasks) {
            System.out.println("Quester.poll");
            if (task.activate()) {
                task.execute();
                break;
            }
        }
    }
}
