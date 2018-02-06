package RS3.quester;

import RS3.Quester;
import RS3.RS3Task;
import org.powerbot.script.Condition;
import org.powerbot.script.Random;
import org.powerbot.script.rt6.ClientContext;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;

public class CheckProgress extends RS3Task {

    List<String> progressTexts = new ArrayList<String>() {{
        add("<col=EBE076>Xenia<col=EB981F>, an old adventurer, said she had seen some Zamorakian cultists entering");
        add("<col=EB981F>I should accompany Xenia to fight the <col=EBE076>first cultist<col=EB981F>.");
        add("<col=EB981F>I need to defeat the <col=EBE076>first cultist<col=EB981F>.");
    }};

    public CheckProgress(ClientContext ctx) {
        super(ctx);
    }

    @Override
    public boolean activate() {
        return Quester.progress == -1;
    }

    @Override
    public void execute() {
        //Open Quest Tab
        while (!ctx.widgets.component(1500, 0).visible()) {
            ctx.input.hop(Random.nextInt(100, 200), Random.nextInt(100, 200));
            ctx.widgets.component(1431, 9).component(3).click();
            Condition.wait(new Callable<Boolean>() {
                @Override
                public Boolean call() throws Exception {
                    return ctx.widgets.component(1500, 0).visible();
                }
            }, 150, 20);
        }

        //Click quest name
        int count = 0;
        while (count++ < 0 && !ctx.widgets.component(1500, 340).text().equals("The Blood Pact")) {
            System.out.println("Clicking name in list");
            ctx.widgets.component(1783, 6).component(170).click();
        }

        //Wait until progress list visible
        Condition.wait(new Callable<Boolean>() {
            @Override
            public Boolean call() throws Exception {
                return ctx.widgets.component(1500, 21).visible();
            }
        }, 150, 20);

        //Check if not started
        if (ctx.widgets.component(1500, 21).text().equals("")) {
            Quester.progress = 0;
        }

        //Check progress
        for (int i = 21; i < 100; i++) {
            System.out.println(ctx.widgets.component(1500, i).text());
            if (ctx.widgets.component(1500, i).text().equals("")) {
                break;
            }

            if (progressTexts.contains(ctx.widgets.component(1500, i).text())) {
                Quester.progress = progressTexts.indexOf(ctx.widgets.component(1500, i).text()) + 1;
            }
        }

        //Close quest tab
        while (ctx.widgets.component(1500, 0).visible()) {
            ctx.widgets.close(ctx.widgets.component(1500, 0), true);
        }
    }
}