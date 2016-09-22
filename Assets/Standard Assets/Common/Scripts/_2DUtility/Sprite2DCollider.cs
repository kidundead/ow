//! @file Sprite2DCollider.cs


using UnityEngine;


//! @class Sprite2DCollider
//! @brief 2D¾«ÁéÅö×²Æ÷
public class Sprite2DCollider
{
	//! Åö×²¼ì²â
	public static bool Collide(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		//
		if ((sprite1.GlobalCollideType == Sprite2D.CollideType.Null) || (sprite2.GlobalCollideType == Sprite2D.CollideType.Null))
		{
			return false;
		}

		// Åö×²ºÐÔ¤Ñ¡
		float minx1, miny1, maxx1, maxy1;
		if (sprite1.WorldRotation == 0)
		{
			Vector2 position = sprite1.WorldPosition;
			Vector2 size = sprite1.Size;
			Vector2 half_size = size / 2;

			minx1 = position.x - half_size.x;
			miny1 = position.y - half_size.y;
			maxx1 = position.x + half_size.x;
			maxy1 = position.y + half_size.y;
		}
		else
		{
			Vector2 position = sprite1.WorldPosition;
			Vector2 size = sprite1.Size;
			float half_size = (size.x >= size.y ? size.x : size.y) * 0.75f;

			minx1 = position.x - half_size;
			miny1 = position.y - half_size;
			maxx1 = position.x + half_size;
			maxy1 = position.y + half_size;
		}

		float minx2 = 0, miny2 = 0, maxx2 = 0, maxy2 = 0;
		if (sprite2.WorldRotation == 0)
		{
			Vector2 position = sprite2.WorldPosition;
			Vector2 size = sprite2.Size;
			Vector2 half_size = size / 2;

			minx2 = position.x - half_size.x;
			miny2 = position.y - half_size.y;
			maxx2 = position.x + half_size.x;
			maxy2 = position.y + half_size.y;
		}
		else
		{
			Vector2 position = sprite2.WorldPosition;
			Vector2 size = sprite2.Size;
			float half_size = (size.x >= size.y ? size.x : size.y) * 0.75f;

			minx1 = position.x - half_size;
			miny1 = position.y - half_size;
			maxx1 = position.x + half_size;
			maxy1 = position.y + half_size;
		}

		float minx = Mathf.Max(minx1, minx2);
		float maxx = Mathf.Min(maxx1, maxx2);
		if (minx > maxx)
		{
			return false;
		}

		float miny = Mathf.Max(miny1, miny2);
		float maxy = Mathf.Min(maxy1, maxy2);
		if (miny > maxy)
		{
			return false;
		}

		// Åö×²¼ì²â
		switch (sprite1.GlobalCollideType)
		{
		case Sprite2D.CollideType.Circle:
			{
				switch (sprite2.GlobalCollideType)
				{
				case Sprite2D.CollideType.Circle:
					return Circle2Circle(sprite1, sprite2, ref collide_position);

				case Sprite2D.CollideType.Rectangle:
					return Circle2Rectangle(sprite1, sprite2, ref collide_position);

				case Sprite2D.CollideType.Polygon:
					return Circle2Polygon(sprite1, sprite2, ref collide_position);
				}
			}
			break;

		case Sprite2D.CollideType.Rectangle:
			{
				switch (sprite2.GlobalCollideType)
				{
				case Sprite2D.CollideType.Circle:
					return Circle2Rectangle(sprite2, sprite1, ref collide_position);

				case Sprite2D.CollideType.Rectangle:
					return Rectangle2Rectangle(sprite1, sprite2, ref collide_position);

				case Sprite2D.CollideType.Polygon:
					return Rectangle2Polygon(sprite1, sprite2, ref collide_position);
				}
			}
			break;

		case Sprite2D.CollideType.Polygon:
			{
				switch (sprite2.GlobalCollideType)
				{
				case Sprite2D.CollideType.Circle:
					return Circle2Polygon(sprite2, sprite1, ref collide_position);

				case Sprite2D.CollideType.Rectangle:
					return Rectangle2Polygon(sprite2, sprite1, ref collide_position);

				case Sprite2D.CollideType.Polygon:
					return Polygon2Polygon(sprite1, sprite2, ref collide_position);
				}
			}
			break;
		}

		return false;
	}

	private static bool Circle2Circle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Circle2Rectangle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Circle2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Rectangle2Rectangle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Rectangle2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Polygon2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}
}

