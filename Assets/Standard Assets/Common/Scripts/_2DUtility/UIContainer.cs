//! @file UIContainer.cs


using UnityEngine;
using System.Collections;


//! @class UIContainer
//! @brief UI容器接口
public interface UIContainer
{
	//! 绘制子控件精灵
	void DrawSprite(UISprite sprite);

	//! 发送子控件事件
	void SendEvent(UIControl control, int command, float wparam, float lparam);


    void Add(UIControl control);
}

