using System;
using UnityEngine;
 
public enum ConnectionPointType { In, Out }
 
public class ConnectionPoint {

    public Rect rect;
    public float verticalPos;
    public ConnectionPointType type;
    public GUIDialogueNode node;
    public GUIStyle style;
    public Action<ConnectionPoint> OnClickConnectionPoint;
    
    public ConnectionPoint(
        GUIDialogueNode node, 
        ConnectionPointType type, 
        GUIStyle style,
        float verticalPos,
        float yPadding,
        Action<ConnectionPoint> OnClickConnectionPoint
    ) {
        this.node = node;
        this.type = type;
        this.style = style;
        // TODO place point in master
        rect = new Rect(0, 0, 10f, 20f);
        this.verticalPos = (node.rect.height - rect.height + verticalPos * node.heightStep + yPadding) * 0.5f;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
    }
 
    public void Draw() {
        rect.y = node.rect.y + verticalPos;
 
        switch (type) {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;
 
            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }
        
        if (GUI.Button(rect, "", style)) {
            if (OnClickConnectionPoint != null) {
                OnClickConnectionPoint(this);
            }
        }
    }
}