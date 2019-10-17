using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision {

    /*
    public static bool HasIntersection ( SHAPE a, SHAPE b ) {
        switch (a.collider) {
            case SHAPE.SHAPES.POINT:
                switch (b.collider) {
                    case SHAPE.SHAPES.POINT:
                        return HasIntersection((POINT)a, (POINT)b);
                    case SHAPE.SHAPES.AABB:
                        return HasIntersection((POINT)a, (AABB)b);
                    case SHAPE.SHAPES.SPHERE:
                        return HasIntersection((POINT)a, (SPHERE)b);
                }

                Debug.LogError("Could not handle collision.");
                return false;
            case SHAPE.SHAPES.AABB:
                switch (b.collider) {
                    case SHAPE.SHAPES.POINT:
                        return HasIntersection((AABB)a, (POINT)b);
                    case SHAPE.SHAPES.AABB:
                        return HasIntersection((AABB)a, (AABB)b);
                    case SHAPE.SHAPES.SPHERE:
                        return HasIntersection((AABB)a, (SPHERE)b);
                }

                Debug.LogError("Could not handle collision.");
                return false;
            case SHAPE.SHAPES.SPHERE:
                switch (b.collider) {
                    case SHAPE.SHAPES.POINT:
                        return HasIntersection((SPHERE)a, (POINT)b);
                    case SHAPE.SHAPES.AABB:
                        return HasIntersection((SPHERE)a, (AABB)b);
                    case SHAPE.SHAPES.SPHERE:
                        return HasIntersection((SPHERE)a, (SPHERE)b);
                }

                Debug.LogError("Could not handle collision.");
                return false;
        }

        Debug.LogError("Could not handle collision.");
        return false;
    }
    */
    /*
    public static bool HasIntersection ( POINT a, POINT b ) {
        Debug.LogWarning("Points cannot intersect.");
        return false;
    }
    */
    public static bool HasIntersection ( AABB b, POINT a ) {
        return  (a.x >= b.minX && a.x <= b.maxX) &&
                (a.y >= b.minY && a.y <= b.maxY) &&
                (a.z >= b.minZ && a.z <= b.maxZ);
    }

    public static bool HasIntersection ( POINT a, AABB b ) {
        return HasIntersection(b, a);
    }
    
    public static bool HasIntersection (AABB a, AABB b) {
        return  (a.minX <= b.maxX && a.maxX >= b.minX) &&
                (a.minY <= b.maxY && a.maxY >= b.minY) &&
                (a.minZ <= b.maxZ && a.maxZ >= b.minZ);
    }

    public static bool HasIntersection ( AABB b, SPHERE s ) {
        float x = Mathf.Max(b.minX, Mathf.Min(s.x, b.maxX));
        float y = Mathf.Max(b.minY, Mathf.Min(s.y, b.maxY));
        float z = Mathf.Max(b.minZ, Mathf.Min(s.z, b.maxZ));

        POINT p = new POINT(x, y, z);

        return HasIntersection(p, s);
    }

    public static bool HasIntersection ( SPHERE s, AABB b ) {
        return HasIntersection(b, s);
    }
    
    public static bool HasIntersection ( POINT p, SPHERE s ) {
        //float sqrDst =  (p.x - s.x) * (p.x - s.x) +
        //                (p.y - s.y) * (p.y - s.y) +
        //                (p.z - s.z) * (p.z - s.z);
        // radius x y en z  naar 1
        // punt x ook naar scale die radius x doet
        p.x = p.x / s.radiusX;
        p.y = p.y / s.radiusY;
        p.z = p.z / s.radiusZ;
        float sqrDst = (p.x - s.x) * (p.x - s.x) +
                    (p.y - s.y) * (p.y - s.y) +
                    (p.z - s.z) * (p.z - s.z);
        return sqrDst < 1;
    }

    public static bool HasIntersection ( SPHERE s, POINT p ) {
        return HasIntersection(p, s);
    }
/*
    public static bool HasIntersection ( SPHERE a, SPHERE b ) {
        float sqrDst =  (a.x - b.x) * (a.x - b.x) +
                        (a.y - b.y) * (a.y - b.y) +
                        (a.z - b.z) * (a.z - b.z);
        return sqrDst < (a.radius * a.radius + b.radius * b.radius);
    }
    */
    /*
    /// <summary>
    /// Basic Shape
    /// </summary>
    [System.Serializable]
    public class SHAPE {

        public enum SHAPES {
            POINT, AABB, SPHERE
        }

        public SHAPES collider;

        public float x;
        public float y;
        public float z;

        public float radius = 0f;

    }
    */

    /// <summary>
    /// Cubular Collision Data Struct (supports 3D)
    /// </summary>
    [System.Serializable]
    public class AABB {

        public float minX;
        public float minY;
        public float minZ;

        public float maxX;
        public float maxY;
        public float maxZ;

        public AABB ( Vector3 centre, Vector3 size ) {
            minX = centre.x - size.x / 2;
            minY = centre.y - size.y / 2;
            minZ = centre.z - size.z / 2;

            maxX = centre.x + size.x / 2;
            maxY = centre.y + size.y / 2;
            maxZ = centre.z + size.z / 2;
        }

        public AABB ( float x, float y, float z, Vector3 size ) {
            minX = x - size.x / 2;
            minY = y - size.y / 2;
            minZ = z - size.z / 2;

            maxX = x + size.x / 2;
            maxY = y + size.y / 2;
            maxZ = z + size.z / 2;
        }

    }

    /// <summary>
    /// Spherical Collision Data Struct (Supports 3D)
    /// </summary>
    [System.Serializable]
    public class SPHERE {

        public float x;
        public float y;
        public float z;
        public float radiusX;
        public float radiusY;
        public float radiusZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;

        public SPHERE ( Vector3 centre, float radius ) {
            this.x = centre.x;
            this.y = centre.y;
            this.z = centre.z;
            this.radiusX = radius;
            this.radiusY = radius;
            this.radiusZ = radius;
            this.rotationX = 0;
            this.rotationY = 0;
            this.rotationZ = 0;
        }

        public SPHERE ( float x, float y, float z, float radiusX, float radiusY,float radiusZ, float rotationX, float rotationY, float rotationZ ) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.radiusX = radiusX;
            this.radiusY = radiusY;
            this.radiusZ = radiusZ;
            this.rotationX = rotationX;
            this.rotationY = rotationY;
            this.rotationZ = rotationZ;
        }

    }

    /// <summary>
    /// Point Collision Data Struct (Supports 3D)
    /// </summary>
    [System.Serializable]
    public class POINT {

        public float x;
        public float y;
        public float z;

        public POINT ( Vector3 centre ) {
            this.x = centre.x;
            this.y = centre.y;
            this.z = centre.z;
        }

        public POINT ( float x, float y, float z ) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

}