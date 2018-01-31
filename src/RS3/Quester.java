package RS3;

import RS3.quester.startQuest.LumbridgeTeleport;
import RS3.quester.startQuest.StartQuest;
import RS3.quester.startQuest.TalkToXenia;
import RS3.quester.startQuest.WalkToXenia;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.PollingScript;
import org.powerbot.script.Script;

import java.util.ArrayList;
import java.util.List;

@Script.Manifest(name="Quester", description = "Does quests", properties = "client=6; author=Marcel; topic=999")

public class Quester extends PollingScript<ClientContext> {

    List<RS3Task> tasks = new ArrayList<RS3Task>();

    @Override
    public void start(){
        tasks.add(new StartQuest(ctx));
    }

    @Override
    public void poll() {
        for (RS3Task task : tasks) {
            if (task.activate()) {
                task.execute();
                break;
            }
        }
    }
}
