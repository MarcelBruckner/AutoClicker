package RS3.quester.startQuest;

import RS3.RS3Task;
import org.powerbot.script.rt6.ClientContext;
import org.powerbot.script.rt6.Component;

import java.util.ArrayList;
import java.util.List;

public class StartQuest extends RS3Task {
    List<RS3Task> tasks = new ArrayList<RS3Task>();

    boolean started = false;
    boolean checked = false;

    public StartQuest(ClientContext ctx) {
        super(ctx);
        tasks.add(new LumbridgeTeleport(ctx));
        tasks.add(new WalkToXenia(ctx));
        tasks.add(new TalkToXenia(ctx));
        tasks.add(new AcceptQuest(ctx));
    }

    @Override
    public boolean activate() {
        System.out.println("Started: " + started);
        System.out.println("Checked: " + checked);

        if(!checked) {
            if(!ctx.widgets.component(1500, 429).visible()) {
                ctx.widgets.component(1431, 9).component(3).click();
            }
            ctx.widgets.component(1783, 6).component(170).click();
            System.out.println("Text: " + ctx.widgets.component(1500, 429).text());
            started = !ctx.widgets.component(1500, 429).text().equals("Not started");
            checked = true;
            ctx.input.send("{VK_ESCAPE down}");
            ctx.input.send("{VK_ESCAPE up}");
        }
        return !started;
    }

    @Override
    public void execute() {
        System.out.println("StartQuest.execute");
        for(RS3Task task : tasks){
            if(task.activate()){
                task.execute();
                break;
            }
        }
    }
}
