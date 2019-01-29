package autoClicker;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Function;

public class MainFrame extends JFrame {

    private JMenuBar menuBar;
    private JTextArea input;
    private JPanel topButtons;
    private JToggleButton record;
    private JToggleButton play;

    private Robot robot;

    public MainFrame() throws AWTException {
        robot = new Robot();
        initUI();
    }

    private void initUI() {

       // createMenuBar();
        createContent();

        setTitle("Simple example");
        setSize(300, 200);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
    }

    private void createContent() {
        input = new JTextArea();
        add(input);
        topButtons = new JPanel(new FlowLayout());
        add(topButtons, BorderLayout.NORTH);
        record = addButton("Record", (event)->record(record), topButtons);
        play = addButton("Play", (event)->{}, topButtons);
        pack();
    }

    private void addButton(String name, ActionListener action){
        addButton(name, action, this);
    }

    private JToggleButton addButton(String name, ActionListener action, Container parent){
        JToggleButton button = new JToggleButton(name);
        button.addActionListener(action);
        parent.add(button);
        return  button;
    }


    private void createMenuBar() {

        menuBar = new JMenuBar();

        addFileMenu();
        addActionsMenu();

        setJMenuBar(menuBar);
    }

    private void addFileMenu(){
        var fileMenu = new JMenu("File");
        fileMenu.setMnemonic(KeyEvent.VK_F);

        var eMenuItem = new JMenuItem("Exit");
        eMenuItem.setMnemonic(KeyEvent.VK_E);
        eMenuItem.setToolTipText("Exit application");
        eMenuItem.addActionListener((event) -> System.exit(0));

        fileMenu.add(eMenuItem);
        menuBar.add(fileMenu);
    }

    private void addActionsMenu(){
        var fileMenu = new JMenu("Actions");
        fileMenu.setMnemonic(KeyEvent.VK_A);

        var eMenuItem = new JMenuItem("Record");
        eMenuItem.setMnemonic(KeyEvent.VK_R);
        eMenuItem.setToolTipText("Start Recording");
//        eMenuItem.addActionListener((event) -> record());

        fileMenu.add(eMenuItem);
        menuBar.add(fileMenu);
    }

    private void record(JToggleButton button){
    }

    public static void main(String[] args) {

        EventQueue.invokeLater(() -> {
            MainFrame ex = null;
            try {
                ex = new MainFrame();
                ex.setVisible(true);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
    }
}
