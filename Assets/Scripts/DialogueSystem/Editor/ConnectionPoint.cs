using System;
using UnityEngine;
using UnityEditor;

public enum ConnectionPointType { In, Out }
 
public class ConnectionPoint {

    private static GUIStyle inPointStyle;
    private static GUIStyle outPointStyle;

    public Rect rect;
    private int verticalPos;
    public ConnectionPointType type;
    public GUIDialogueNode node;
    public GUIStyle style;
    public Action<ConnectionPoint> OnClickConnectionPoint;
    
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
        GUIDialogueNode node, 
        ConnectionPointType type,
        Action<ConnectionPoint> OnClickConnectionPoint,
        int verticalPos = 0
    ) {
        this.node = node;
        this.type = type;
        if(type == ConnectionPointType.In) {
            style = inPointStyle;
        } else {
            style = outPointStyle;
        }
        rect = new Rect(0, 0, 10f, 20f);
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.verticalPos = verticalPos;
    }
 
    public void Draw() {
        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                rect.y = node.rect.y + (GUIDialogueNode.mainBlockHeight - rect.height) / 2;
                break;
 
            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                rect.y = node.rect.y + 
                    GUIDialogueNode.mainBlockHeight + 
                    (GUIDialogueNode.textHeight + GUIDialogueNode.padding) * verticalPos +
                    (GUIDialogueNode.textHeight - rect.height) / 2;
                break;
        }
        
        if (GUI.Button(rect, "", style)) {
            if (OnClickConnectionPoint != null) {
                OnClickConnectionPoint(this);
            }
        }
    }
}