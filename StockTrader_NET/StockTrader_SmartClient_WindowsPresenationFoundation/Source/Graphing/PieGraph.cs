using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace StockTrader.Graphing
{
    public class PieElement
    {
        public string label;
        public double value;
        public PieElement(string name, double val)
        {
            label = name;
            value = val;
        }
    }

    class PieGraph : Viewport3D
    {
        internal const double Radius = 4d;
        internal const double PI2 = Math.PI * 2;
        List<PiePiece> PiePieces;
        public List<PieElement> Data;
        private double HitDistanceMove;
        private double HitDistanceClick;

        private PiePiece MovePiece;
        private PiePiece ClickPiece;
        public double Sum;

        List<Color> ColorList;

        public PieGraph()
        {
            ColorList = new List<Color>();
            ColorList.Add(Color.FromRgb(255, 91, 43));
            ColorList.Add(Color.FromRgb(177, 34, 28));
            ColorList.Add(Color.FromRgb(52, 57, 62));
            ColorList.Add(Color.FromRgb(140, 198, 215));
            ColorList.Add(Color.FromRgb(255, 218, 140));
            //System.Windows.Media.Effects.DropShadowBitmapEffect shadow = new System.Windows.Media.Effects.DropShadowBitmapEffect();
            //BitmapEffect = shadow;

            MouseMove += new System.Windows.Input.MouseEventHandler( onMouseMove );
            MouseLeave += new System.Windows.Input.MouseEventHandler( onMouseLeave );

            MouseDown += new MouseButtonEventHandler( onMouseDown );
        }

        public void setData( List<PieElement> data )
        {
            if( Children.Count > 0 )
                Children.RemoveAt(0);
            Data = data;
            Sum = 0d;
            foreach (PieElement element in Data)
                Sum += element.value;


            Model3DGroup group = new Model3DGroup();
            group.Children.Add( new AmbientLight( Color.FromRgb( 128, 128, 128) ) );
            group.Children.Add( new DirectionalLight( Colors.White, new Vector3D( 1, 1, 1 ) ) );

            PiePieces = new List<PiePiece>();
            double cCurrent = 0;
            int idx = 0;
            foreach (PieElement element in Data)
            {
                PiePiece piece = new PiePiece(cCurrent, (cCurrent += element.value / Sum * PI2), ColorList[idx%ColorList.Count]);
                PiePieces.Add( piece );
                group.Children.Add( piece.Geometry );
                idx++;
            }

            ModelVisual3D visual = new ModelVisual3D();
            visual.Content = group;

            PerspectiveCamera camera = new PerspectiveCamera();
            camera.FarPlaneDistance = 30;
            camera.NearPlaneDistance = 1;
            camera.FieldOfView = 90;
            camera.Position = new Point3D (0, -6.5, -4.5 );
            camera.LookDirection = new Vector3D( 0, 7.5, 5.5 );
            camera.UpDirection = new Vector3D( 0, 1, 0 );

            Children.Add( visual );
            Camera = camera;
        }

        private void onMouseLeave( object obj, MouseEventArgs e )
        {
            if (MovePiece != null)
            {
                MovePiece.onRollOut();
                MovePiece = null;
            }
        }

        private void onMouseMove( object obj, MouseEventArgs e )
        {
            Point pos = e.GetPosition( this );
            PointHitTestParameters pointparams = new PointHitTestParameters( pos );

            HitDistanceMove = double.MaxValue;
            PiePiece lastMovePiece = MovePiece;
            MovePiece = null;

            VisualTreeHelper.HitTest( this, null, onGeometryMoveHit, pointparams );

            if( MovePiece != lastMovePiece )
            {
                if( MovePiece != null )
                    MovePiece.onRollOver();
                if (lastMovePiece != null)
                    lastMovePiece.onRollOut();
            }
        }

        public void SelectPiece(int id)
        {
            if (MovePiece != null)
                MovePiece.Select(false);
            if (id != -1 && PiePieces.Count > id && PiePieces[id] != null)
                PiePieces[id].Select(true);
            MovePiece = id == -1 ? null : PiePieces[id];
        }

        private void onMouseDown( object obj, MouseEventArgs e )
        {
            Point pos = e.GetPosition( this );
            PointHitTestParameters pointparams = new PointHitTestParameters( pos );

            HitDistanceClick = double.MaxValue;
            PiePiece lastClickPiece = ClickPiece;
            ClickPiece = null;

            VisualTreeHelper.HitTest( this, null, onGeometryClickHit, pointparams );

            if( ClickPiece != lastClickPiece )
            {
                if (ClickPiece != null)
                    ClickPiece.Select(true);
                if( lastClickPiece != null )
                    lastClickPiece.Select( false );

                //Select.cEnd
            }
        }

        private HitTestResultBehavior onGeometryMoveHit( System.Windows.Media.HitTestResult result )
        {
            RayMeshGeometry3DHitTestResult hit = result as RayMeshGeometry3DHitTestResult;
            if (hit != null && hit.DistanceToRayOrigin < HitDistanceMove && hit.ModelHit is GeometryModel3D)
            {
                GeometryModel3D model = hit.ModelHit as GeometryModel3D;
                foreach( PiePiece piece in PiePieces )
                {
                    if( piece.Geometry == model )
                    {
                        HitDistanceMove = hit.DistanceToRayOrigin;
                        MovePiece = piece;
                        break;
                    }
                }
            }

            return HitTestResultBehavior.Continue;
        }

        private HitTestResultBehavior onGeometryClickHit( System.Windows.Media.HitTestResult result )
        {
            RayMeshGeometry3DHitTestResult hit = result as RayMeshGeometry3DHitTestResult;
            if (hit != null && hit.DistanceToRayOrigin < HitDistanceClick && hit.ModelHit is GeometryModel3D)
            {
                GeometryModel3D model = hit.ModelHit as GeometryModel3D;
                foreach( PiePiece piece in PiePieces )
                {
                    if( piece.Geometry == model )
                    {
                        HitDistanceClick = hit.DistanceToRayOrigin;
                        ClickPiece = piece;
                        break;
                    }
                }
            }

            return HitTestResultBehavior.Continue;
        }
    }

    class PiePiece
    {
        internal const double Step = PieGraph.PI2 / 360d;
        GeometryModel3D geometry;
        MeshGeometry3D mesh;

        public double cStart;
        public double cEnd;
        Color color;
        Color overColor;

        public PiePiece( double start, double end, Color c )
        {
            cStart = start;
            cEnd = end;
            color = c;
            overColor = Color.FromRgb(200, 200, 200);//(byte)Math.Min(255, c.R + 60), (byte)Math.Min(255, c.G + 60), (byte)Math.Min(255, c.B + 60));

            geometry = new GeometryModel3D();
            mesh = new MeshGeometry3D();

            mesh.Positions.Add( new Point3D( 0, 0, 0 ) );

            // Create the top.
            double rad = cStart;
            int steps = 0;
            while( true )
            {
                double x = PieGraph.Radius * Math.Sin( rad );
                double y = PieGraph.Radius * Math.Cos( rad );
                mesh.Positions.Add(new Point3D(x, y, 0));
                if( rad == cEnd )
                    break;
                rad += Step;
                if( rad > cEnd )
                    rad = cEnd;
                steps++;
            }

            for( int i = 1; i <= steps; i++ )
            {
                mesh.TriangleIndices.Add( 0 );
                mesh.TriangleIndices.Add( i );
                mesh.TriangleIndices.Add( i + 1 );
            }

            // Create the bottom.
            mesh.Positions.Add(new Point3D( 0, 0, 1) );
            int placeMarker = steps + 2;
            mesh.TriangleIndices.Add( 0 );
            mesh.TriangleIndices.Add( placeMarker );
            mesh.TriangleIndices.Add( placeMarker );

            rad = cStart;
            steps = 0;
            while( true )
            {
                double x = PieGraph.Radius * Math.Sin( rad );
                double y = PieGraph.Radius * Math.Cos( rad );
                mesh.Positions.Add( new Point3D( x, y, 1 ) );
                if( rad == cEnd )
                    break;
                rad += Step;
                if( rad > cEnd )
                    rad = cEnd;
                steps++;
            }

            for( int i = placeMarker + 1; i <= steps + placeMarker; i++ )
            {
                mesh.TriangleIndices.Add( placeMarker );
                mesh.TriangleIndices.Add(i + 1);
                mesh.TriangleIndices.Add(i);
            }

            // Create the outer edge.
            for (int i = 1, j = placeMarker + 1; i < steps; i++, j++)
            {
                mesh.TriangleIndices.Add(i);
                mesh.TriangleIndices.Add(j);
                mesh.TriangleIndices.Add(j + 1);
                mesh.TriangleIndices.Add(j + 1);
                mesh.TriangleIndices.Add(i + 1);
                mesh.TriangleIndices.Add(i);
            }

            // Create the ending edge.
            mesh.TriangleIndices.Add( steps );
            mesh.TriangleIndices.Add( steps + placeMarker );
            mesh.TriangleIndices.Add( placeMarker );
            mesh.TriangleIndices.Add( placeMarker );
            mesh.TriangleIndices.Add( 0 );
            mesh.TriangleIndices.Add( steps );

            // Create the leading edge.
            mesh.TriangleIndices.Add( 0 );
            mesh.TriangleIndices.Add( placeMarker );
            mesh.TriangleIndices.Add( placeMarker + 1 );
            mesh.TriangleIndices.Add( placeMarker + 1 );
            mesh.TriangleIndices.Add( 1 );
            mesh.TriangleIndices.Add( 0 );

            geometry.Geometry = mesh;
            MaterialGroup material = new MaterialGroup();
            material.Children.Add( new DiffuseMaterial( new SolidColorBrush( color ) ) );
            material.Children.Add( new SpecularMaterial (new SolidColorBrush( Colors.White ), 85 ) );
            geometry.Material = material;
        }

        public GeometryModel3D Geometry
        {
            get 
            { 
                return geometry; 
            }
        }

        public void Select( bool s )
        {
            Transform3DGroup xform = null;
            if( !( geometry.Transform is Transform3DGroup ) )
            {
                xform = new Transform3DGroup();
                geometry.Transform = xform;
            }
            else
                xform = geometry.Transform as Transform3DGroup;

            TranslateTransform3D translate = new TranslateTransform3D( 0, 0, 0 );
            xform.Children.Add( translate );
            
            double x = -PieGraph.Radius * Math.Sin(cStart+(cEnd-cStart)/2.0);
            double y = -PieGraph.Radius * Math.Cos(cStart+(cEnd-cStart)/2.0);

            DoubleAnimationUsingKeyFrames dax = new DoubleAnimationUsingKeyFrames();
            dax.Duration = TimeSpan.FromMilliseconds(500);
            dax.KeyFrames.Add(new SplineDoubleKeyFrame(s ? -0.25 * x : 0.25 * x, TimeSpan.FromMilliseconds(500), new KeySpline(0.0, 0.6, 0.0, 0.9)));

            DoubleAnimationUsingKeyFrames day = new DoubleAnimationUsingKeyFrames();
            day.Duration = TimeSpan.FromMilliseconds(500);
            day.KeyFrames.Add(new SplineDoubleKeyFrame(s ? -0.25 * y : 0.25 * y, TimeSpan.FromMilliseconds(500), new KeySpline(0.0, 0.6, 0.0, 0.9)));

            DoubleAnimationUsingKeyFrames daz = new DoubleAnimationUsingKeyFrames();
            daz.Duration = TimeSpan.FromMilliseconds(500);
            daz.KeyFrames.Add(new SplineDoubleKeyFrame(s ? -2 : 2, TimeSpan.FromMilliseconds(500), new KeySpline(0.0, 0.6, 0.0, 0.9)));

            translate.BeginAnimation(TranslateTransform3D.OffsetXProperty, dax);
            translate.BeginAnimation(TranslateTransform3D.OffsetYProperty, day);
            translate.BeginAnimation(TranslateTransform3D.OffsetZProperty, daz);
        }

        public void onRollOver()
        {
            ColorAnimation ca = new ColorAnimation(overColor, new Duration(new TimeSpan(2500000)));
            MaterialGroup matg = geometry.Material as MaterialGroup;
            foreach (Material m in matg.Children)
            {
                if (m is DiffuseMaterial)
                {
                    DiffuseMaterial dm = m as DiffuseMaterial;
                    dm.Brush.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                    break;
                }
            }
        }

        public void onRollOut()
        {
            ColorAnimation ca = new ColorAnimation(color, new Duration(new TimeSpan(2500000)));
            MaterialGroup matg = geometry.Material as MaterialGroup;
            foreach (Material m in matg.Children)
            {
                if (m is DiffuseMaterial)
                {
                    DiffuseMaterial dm = m as DiffuseMaterial;
                    dm.Brush.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                    break;
                }
            }
        }
    }
}
