﻿using System;
using Microsoft.Xna.Framework;
using Nez.PhysicsShapes;


namespace Nez
{
	/// <summary>
	/// Polygons should be defined in clockwise fashion.
	/// </summary>
	public class PolygonCollider : Collider
	{
		public PolygonCollider( Vector2[] points )
		{
			// first and last point must not be the same. we want an open polygon
			var isPolygonClosed = points[0] == points[points.Length - 1];

			if( isPolygonClosed )
				Array.Resize( ref points, points.Length - 1 );

			shape = new Polygon( points );
		}


		public PolygonCollider( int vertCount, float radius )
		{
			shape = new Polygon( vertCount, radius );
		}


		public override void debugRender( Graphics graphics )
		{
			var poly = shape as Polygon;
			graphics.batcher.drawHollowRect( bounds, DefaultColors.colliderBounds );
			graphics.batcher.drawPolygon( shape.position, poly.points, DefaultColors.colliderEdge, true );
			graphics.batcher.drawPixel( entity.transform.position, DefaultColors.colliderPosition, 4 );
			graphics.batcher.drawPixel( shape.position, DefaultColors.colliderCenter, 2 );
		}

	}
}

