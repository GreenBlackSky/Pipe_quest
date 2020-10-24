using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ConnectionPointType { In, Out }
 
public class ConnectionPoint {

    private static GUIStyle inPointStyle;
    private static GUIStyle outPointStyle;

    public Rect rect;
    public int verticalPos;
    public ConnectionPointType type;
    public GUIDialogueNode parent;
    public GUIStyle style;
    public HashSet<Connection> connections;
    
    public static void initStyles() {
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);
 
        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    public ConnectionPoint(
        GUIDialogueNode parent, 
        ConnectionPointType type,
        int verticalPos = 0
    ) {
        this.parent = parent;
        this.type = type;
        connections = new HashSet<Connection>();
        if(type == ConnectionPointType.In) {
            style = inPointStyle;
        } else {
            style = outPointStyle;
        }
        rect = new Rect(0, 0, 10f, 20f);
        this.verticalPos = verticalPos;
    }
 
    public void Draw() {
        switch (type) {
            case ConnectionPointType.In:
                rect.x = parent.rect.x - rect.width + 8f;
                rect.y = parent.rect.y + (GUIDialogueNode.mainBlockHeight - rect.height) / 2;
                break;
 
            case ConnectionPointType.Out:
                rect.x = parent.rect.x + parent.rect.width - 8f;
                rect.y = parent.rect.y + 
                    GUIDialogueNode.mainBlockHeight + 
                    (GUIDialogueNode.textHeight + GUIDialogueNode.padding) * verticalPos +
                    (GUIDialogueNode.textHeight - rect.height) / 2;
                break;
        }
        
        if (GUI.Button(rect, "", style)) {
            if(type == ConnectionPointType.In) {
                parent.editor.OnClickInPoint(this);
            } else {
                parent.editor.OnClickOutPoint(this);
            }
        }
    }
}