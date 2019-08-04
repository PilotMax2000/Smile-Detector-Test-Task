
namespace SmileDetectorTestTask
{
    [System.Serializable]
    public class FaceRectangle
    {
        public int top;
        public int left;
        public int width;
        public int height;
    }
    [System.Serializable]
    public class FaceAttributes
    {
        public double smile;
    }

    [System.Serializable]
    public class FaceDetectionData
    {
        public string faceId;
        public FaceRectangle faceRectangle;
        public FaceAttributes faceAttributes;
    }

}
