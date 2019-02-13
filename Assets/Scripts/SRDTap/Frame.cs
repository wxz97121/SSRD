using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// time mark
public class Timestamp{
	public int start;
	public int end;
	public Timestamp (int a,int b){
		start = a;
		end = b;
	}
}
// part of shape from .start to .end
public class Progress{
	public float start;
	public float end;
	public Progress (float a,float b){
		start = a;
		end = b;
	}
	public static Progress operator- (Progress a, Progress b){
		Progress c = new Progress (a.start - b.start, a.end - b.end);
		return c;
	}
}
// list of rotation action 
public class Rotation{
	public Vector3 center;
	public float angle;
	public Rotation (Vector3 a,float b){
		center = a;
		angle = b;
	}
	public Rotation (float a,float b,float c,float d){
		center = new Vector3 (a, b, c);
		angle = d;
	}
}

public class Frame{
	Timestamp time;
	Vector3 pivot;
	Vector3[] point;
	float size;
	float radius;
	float arc;
	Rotation[] rotation;
	Color color;
	float alpha;
	float weight;
	Progress progress;
	bool linear;
	bool outline;
	bool connect;
	Tweenmode tweenmode;

	Vector3[] vertices;
	int[] triangles;

	int circleSegment;
	int circleOffset;

	public Frame(){
		time = new Timestamp (0, 12);
		pivot = new Vector3 (0, 0, 0);
		point = new Vector3[0];
		size = 1.0f;
		radius = 1.0f;
		arc = 2 * Mathf.PI;
		rotation = new Rotation[0];
		color = Color.white;
		alpha = 1.0f;
		weight = 0.2f;
		progress = new Progress (0.2f, 1.0f);
		linear = true;
		outline = true;
		connect = true;
		tweenmode = Tweenmode.Linear;

		//other parameter
		circleSegment = 360;
		circleOffset = 3;
	}
	public Frame(Frame f){
		time = new Timestamp (f.getTime().start,f.getTime().end);
		pivot = new Vector3 (f.getPivot().x, f.getPivot().y, f.getPivot().z);
		point = new Vector3[f.getPoint().Length];
		size = f.size;
		radius = f.radius;
		arc = f.arc;
		rotation = new Rotation[f.getRotation().Length];
		for (int i = 0; i < f.getRotation ().Length; i++) {
			rotation [i] = new Rotation (f.getRotation(i).center, f.getRotation(i).angle);
		}
		color = f.color;
		alpha = f.alpha;
		weight = f.weight;
		progress = new Progress (f.getProgress().start, f.getProgress().end);
		linear = f.linear;
		outline = f.outline;
		connect = f.connect;
		tweenmode = f.tweenmode;

		//other parameter
		circleSegment = 360;
		circleOffset = 3;
	}

	public Frame(int _time_start, int _time_end, float _pivot_x, float _pivot_y, Vector3[] _point_list, float _size, float _radius, float _arc, Rotation[] _rotation_list, Color _color, float _alpha, float _weight, float _progress_start, float _progress_end, bool _is_linear, bool _is_outline, bool _is_connect, Tweenmode _tweenmode){
		time = new Timestamp (_time_start, _time_end);
		pivot = new Vector3 (_pivot_x, _pivot_y, 0);
		point = _point_list;
		size = _size;
		radius = _radius;
		arc = _arc;
		rotation = _rotation_list;
		color = _color;
		alpha = _alpha;
		weight = _weight;
		progress = new Progress (_progress_start, _progress_end);
		linear = _is_linear;
		outline = _is_outline;
		connect = _is_connect;
		tweenmode = _tweenmode;

		//other parameter
		circleSegment = 360;
		circleOffset = 3;
	}

	public Timestamp getTime(){
		return time;
	}
	public Vector3 getPivot(){
		return pivot;
	}
	public void setPivot(Vector3 p){
		pivot.x = p.x;
		pivot.y = p.y;
	}
	public Vector3[] getPoint(){
		return point;
	}
	public Vector3 getPoint(int i){
		return point[i];
	}
	public void setPoint(Vector3 p, int i){
		point [i].x = p.x;
		point [i].y = p.y;
	}
	public void setPoint(float xx, float yy, int i){
		point [i].x = xx;
		point [i].y = yy;
	}
	public float getSize (){
		return size;
	}
	public void setSize (float a){
		size = a;
	}
	public float getRadius (){
		return radius;
	}
	public void setRadius (float a){
		radius = a;
	}
	public float getArc (){
		return arc;
	}
	public void setArc (float a){
		arc = a;
	}
	public Rotation[] getRotation(){
		return rotation;
	}
	public Rotation getRotation(int i){
		return rotation[i];
	}
	public float getRotationAngle(int i){
		return rotation[i].angle;
	}
	public void setRotationAngle(float a,int i){
		rotation [i].angle = a;
	}
	public float getAlpha (){
		return alpha;
	}
	public void setAlpha (float a){
		alpha = a;
	}
	public Progress getProgress (){
		return progress;
	}
	public void setProgress (Progress a){
		progress = a;
	}
	public float getBorderWeight(){
		return weight;
	}
	public Color getColor(){
		Color c = new Color (color.r,color.g,color.b,alpha);
		return c;
	}
	public Tweenmode getTweenmode(){
		return tweenmode;
	}
	public Vector3[] getVertices (){
		return vertices;
	}
	public int[] getTriangles (){
		return triangles;
	}

	// generate vertices and triangles
	public void apply(){
		if (linear) {
			if (outline) {
				if (connect) {
					vertices = new Vector3[point.Length * 2];
					for (int i = 0; i < point.Length - 1; i++) {
						vertices [i * 2] = point [i];
						vertices [i * 2 + 1] = point [i + 1]; 
					}
					vertices [point.Length * 2 - 2] = point [point.Length - 1];
					vertices [point.Length * 2 - 1] = point [0];
				} 
				else {
					vertices = new Vector3[(point.Length - 1) * 2];
					for (int i = 0; i < point.Length - 1; i++) {
						vertices [i * 2] = point [i];
						vertices [i * 2 + 1] = point [i + 1]; 
					}
				}
				List<Vector3> newVertices = new List<Vector3>();
				// get a part of line
				if (progress.start > 1) {
					lineProgress (progress.start - 1, progress.end - 1, newVertices);
				} 
				else {
					if (progress.end > 1) {
						lineProgress (progress.start, 1, newVertices);
						lineProgress (0, progress.end, newVertices);
					} 
					else {
						lineProgress (progress.start, progress.end, newVertices);
					}
				}
				vertices = newVertices.ToArray ();

				//add weight to line
				lineAddWeight ();
			} 
			else {
				if (connect) {
					vertices = new Vector3[point.Length];
					for (int i = 0; i < point.Length; i++) {
						vertices [i] = point [i];
					}
					triangles = new int[(point.Length - 2) * 3];
					for (int i = 0; i < point.Length  - 2; i++) {
						triangles [i * 3] = 0;
						triangles [i * 3 + 1] = i + 1;
						triangles [i * 3 + 2] = i + 2;
					}
				} else {
					vertices = new Vector3[point.Length];
					for (int i = 0; i < point.Length; i++) {
						vertices [i] = point [i];
					}
					triangles = new int[(point.Length - 2) * 3];
					for (int i = 0; i < point.Length - 2; i++) {
						triangles [i * 3] = i;
						triangles [i * 3 + 1] = i + 1;
						triangles [i * 3 + 2] = i + 2;
					}	
				}
			}
		} 
		else {
			if (outline) {
				vertices = new Vector3[circleSegment*2];  
				vertices[0] =  new Vector3(0,0,0);  
				float deltaAngle = Mathf.PI * 2 / circleSegment ;  
				int curSegment = (int) (arc / deltaAngle);
				if (Mathf.Abs (circleSegment - curSegment) < circleOffset) {
					curSegment = circleSegment;
				}
				float currentAngle = 0;
				for (int i = 0; i < circleSegment; i++)  
				{  
					float cosA = Mathf.Cos(currentAngle);  
					float sinA = Mathf.Sin(currentAngle);  
					vertices[2*i] = new Vector3(cosA * (radius+weight/2), sinA * (radius+weight/2), 0);
					vertices[2*i+1] = new Vector3(cosA * (radius-weight/2), sinA * (radius-weight/2), 0);  
					currentAngle += deltaAngle;  
				} 
				triangles = new int[curSegment * 6];  

				for (int i = 0; i < curSegment; i++)
				{  
					if (i < circleSegment - 1) {
						triangles [6*i] = 2*i;
						triangles [6*i + 1] = 2*i + 1;
						triangles [6*i + 2] = 2*i + 2;
						triangles [6*i + 3] = 2*i + 1;
						triangles [6*i + 4] = 2*i + 2;
						triangles [6*i + 5] = 2*i + 3;
					} 
					if (i == circleSegment - 1) {
						triangles [6*i] = 2*i;
						triangles [6*i + 1] = 2*i + 1;
						triangles [6*i + 2] = 0;
						triangles [6*i + 3] = 2*i + 1;
						triangles [6*i + 4] = 0;
						triangles [6*i + 5] = 1;
					}

				}
			} 
			else {
				vertices = new Vector3[circleSegment + 1];  
				vertices[0] = new Vector3(0,0,0);  
				float deltaAngle = Mathf.PI * 2 / circleSegment ;  
				int curSegment = (int) (arc / deltaAngle);
				if (Mathf.Abs (circleSegment - curSegment) < circleOffset) {
					curSegment = circleSegment;
				}
				float currentAngle = 0;
				for (int i = 1; i < vertices.Length; i++)  
				{  
					float cosA = Mathf.Cos(currentAngle);  
					float sinA = Mathf.Sin(currentAngle);  
					vertices[i] = new Vector3(cosA * radius, sinA * radius, 0);  
					currentAngle += deltaAngle;  
				} 

				triangles = new int[curSegment * 3];  

				for (int i = 0; i < curSegment; i++)  
				{  
					if (i < circleSegment - 1) {
						triangles [3 * i] = 0;
						triangles [3 * i + 1] = i + 1;
						triangles [3 * i + 2] = i + 2;
					} 
					if (i == circleSegment - 1) {
						triangles [3 * i] = 0;
						triangles [3 * i + 1] = i + 1;
						triangles [3 * i + 2] = 1;
					}
				}
			}
		}

		sizeDeform ();
		shapeRotate ();
		addPivot ();
	}

	void lineProgress (float a, float b, List<Vector3> newVertices){
		float totalLength = 0;
		for (int i = 0; i < vertices.Length / 2; i++) {
			totalLength += pointDistance (vertices [i * 2], vertices [i * 2 + 1]);
		}
		float startPosition = a * totalLength;
		float endPosition = b * totalLength;
		float startLength = 0;
		float endLength = 0;
		Vector3 newP1, newP2;
		for (int i = 0; i < vertices.Length / 2; i++) {
			endLength = startLength + pointDistance (vertices [i * 2], vertices [i * 2 + 1]);
			if (startLength <= startPosition) {
				if (endLength <= startPosition) {
					//do nothing
				} 
				else if (endLength <= endPosition) {
					newP1 = pointFromLine (vertices [i * 2], vertices [i * 2 + 1], (startPosition - startLength) / (endLength - startLength));
					newP2 = vertices [i * 2 + 1];
					newVertices.Add (newP1);
					newVertices.Add (newP2);
				} 
				else {
					newP1 = pointFromLine (vertices [i * 2], vertices [i * 2 + 1], (startPosition - startLength) / (endLength - startLength));
					newP2 = pointFromLine (vertices [i * 2], vertices [i * 2 + 1], (endPosition - startLength) / (endLength - startLength));
					newVertices.Add (newP1);
					newVertices.Add (newP2);
				}
			} 
			else if (startLength <= endPosition) {
				if (endLength <= endPosition) {
					newP1 = vertices [i * 2];
					newP2 = vertices [i * 2 + 1];
					newVertices.Add (newP1);
					newVertices.Add (newP2);
				} 
				else {
					newP1 = vertices [i * 2];
					newP2 = pointFromLine (vertices [i * 2], vertices [i * 2 + 1], (endPosition - startLength) / (endLength - startLength));
					newVertices.Add (newP1);
					newVertices.Add (newP2);
				}
			}
			startLength = endLength;
		}
	}

	void lineAddWeight (){
		Vector3[] newVertices = new Vector3[vertices.Length * 2];
		int[] newTriangles = new int[vertices.Length * 3];
		for (int i = 0; i < vertices.Length / 2; i++) {
			Vector3 v1, v2, v3, v4, v5, v6;
			if (i == 0) {
				v1 = vertices [vertices.Length - 2];
				v2 = vertices [vertices.Length - 1];
			} 
			else {
				v1 = vertices [2 * i - 2];
				v2 = vertices [2 * i - 1];
			}
			v3 = vertices [2 * i];
			v4 = vertices [2 * i + 1];
			if (i == vertices.Length/2 - 1) {
				v5 = vertices [0];
				v6 = vertices [1];
			} 
			else {
				v5 = vertices [2 * i + 2];
				v6 = vertices [2 * i + 3];
			}

			float aa = pointAngle (v3, v4);
			newVertices [4 * i] = pointFromAngle(vertices [2 * i], weight/Mathf.Sqrt(2), aa + Mathf.PI/4);
			newVertices [4 * i + 2] = pointFromAngle(vertices [2 * i], weight/Mathf.Sqrt(2), aa - Mathf.PI/4);
			newVertices [4 * i + 1] = pointFromAngle(vertices [2 * i + 1], weight/Mathf.Sqrt(2), aa - Mathf.PI/4 + Mathf.PI);
			newVertices [4 * i + 3] = pointFromAngle(vertices [2 * i + 1], weight/Mathf.Sqrt(2), aa + Mathf.PI/4 + Mathf.PI);

			newTriangles [6 * i] = 4 * i;
			newTriangles [6 * i + 1] = 4 * i + 1;
			newTriangles [6 * i + 2] = 4 * i + 2;
			newTriangles [6 * i + 3] = 4 * i + 1;
			newTriangles [6 * i + 4] = 4 * i + 2;
			newTriangles [6 * i + 5] = 4 * i + 3;
		}
		vertices = newVertices;
		triangles = newTriangles;
	}
	float pointAngle (Vector3 a, Vector3 b){
		float an;
		float xx = a.x - b.x;
		float yy = a.y - b.y;

		if (Mathf.Abs (xx) < 0.001f) {
			an = Mathf.PI / 2f;
		} 
		else {
			an = Mathf.Atan(Mathf.Abs(yy/xx));
		}

		if ((xx < 0.0f) && (yy >= 0.0f))  
			an = Mathf.PI - an;  
		else if ((xx < 0.0f) && (yy < 0.0f))  
			an = Mathf.PI + an;  
		else if ((xx >= 0.0f) && (yy < 0.0f))  
			an = Mathf.PI * 2.0f - an;  

		return an;
	}
	float pointDistance (Vector3 a, Vector3 b){
		return Mathf.Sqrt (Mathf.Pow (a.x - b.x, 2) + Mathf.Pow (a.y - b.y, 2));
	}
	Vector3 pointFromLine (Vector3 a, Vector3 b, float rate){
		return a + (b - a) * rate;
	}
	Vector3 pointFromAngle (Vector3 a, float r, float an){
		return a + r * new Vector3 (Mathf.Cos(an),Mathf.Sin(an),0);
	}
	bool pointEqual (Vector3 a, Vector3 b, float t){
		if ((Mathf.Abs (a.x - b.x) <= t) && (Mathf.Abs (a.y - b.y) <= t)) {
			return true;
		}
		return false;
	}
	Vector3 pointRotate (Vector3 point, Vector3 center, float angle){
		float r = pointDistance (point, center);
		float an = pointAngle (point, center) + angle;
		Vector3 p = new Vector3 (center.x + r * Mathf.Cos (an), center.y + r * Mathf.Sin (an), 0);
		return p;

	}
	void sizeDeform (){
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i] = vertices [i] * size;
		}
	}

	void shapeRotate (){
		for (int i = 0; i < rotation.Length; i++) {
			for (int j = 0; j < vertices.Length; j++) {
				vertices [j] = pointRotate (vertices[j], rotation [i].center, rotation [i].angle);
			}
		}
	}

	void addPivot (){
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i] = vertices [i] + pivot;
		}
	}

	public static Vector3[] getPolygonPoint (int edge, float radius){
		float angle = 2f * Mathf.PI / (float)edge;
		Vector3[] point = new Vector3[edge];
		for (int i = 0; i < edge; i++) {
			point [i] = new Vector3 (radius*Mathf.Cos(angle*(float)(i)),radius*Mathf.Sin(angle*(float)(i)),0);
		}
		return point;
	}

	public static Vector3[] getRandomPoint (int count, float width, float height){
		Vector3[] point = new Vector3[count];
		for (int i = 0; i < count; i++) {
			point [i] = new Vector3 (Random.Range(-width,width),Random.Range(-height,height),0);
		}
		return point;
	}
}


