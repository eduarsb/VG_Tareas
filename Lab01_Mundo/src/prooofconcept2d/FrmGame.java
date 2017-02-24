/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package prooofconcept2d;
import java.awt.Canvas;
import java.awt.Color;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JFrame;

/**
 *
 * @author servkey
 */
public class FrmGame extends JFrame{
    private Thread t;
    private CanvasDib c;
    public FrmGame(CanvasDib c){
       
        this.c = c;
        add(c);
        //Iniciar hilo
        t = new Thread(){
              public void run(){
                  updating();
              }
        };
        t.start();
     
        this.getContentPane().setBackground(Color.WHITE);
    }
    
    public void updating(){
        while (true){
            try {
                Thread.sleep(100);
                c.repaint();
               
            } catch (InterruptedException ex) {
                Logger.getLogger(FrmGame.class.getName()).log(Level.SEVERE, null, ex);
            }           
        }
    }
}
