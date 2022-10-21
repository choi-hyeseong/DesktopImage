using System.Collections.Generic;

namespace DesktopImage {

    interface IMovingStrategy {
        bool CanMove();

        Point Move();

        void ChangeDirection();
    }

    public enum Move {
        STATIC,
        UPDOWN,
        LEFTRIGHT,
        BOUNCE,
        RANDOM
    }

    public class Twin<T> {

        //좌표 2개 받기위한 클래스

        public T first {
            get;
        }
        public T second {
            get;
        }

        public Twin(T first, T second) {
            this.first = first;
            this.second = second;
        }


    }

    public abstract class MovingStrategy : IMovingStrategy {

        protected Point direction; //직접 지정
        protected Twin<Point>? formPoint; //폼의 크기
        protected Size desktopSize; //화면 최대크기

        public virtual bool CanMove() {
            throw new NotImplementedException();
        }

        public void setBound(Twin<Point> cornerPoint, Size desktopSize) {
            formPoint = cornerPoint;
            this.desktopSize = desktopSize;
        }

        public virtual void ChangeDirection() {
            throw new NotImplementedException();
        }

        public Point Move() {
            Point first = formPoint.first;
            return new Point(first.X + direction.X, first.Y + direction.Y);
        }

    }

    public class UpDownStrategy : MovingStrategy { //상속, 구현할때는 implements가 아니라 : 

        public UpDownStrategy() {
            direction = new Point(0, 1);
        }


        public override bool CanMove() {
            Point first = formPoint.first;
            Point second = formPoint.second;
            if (((first.Y + direction.Y < 0) && direction.Y < 0) || ((second.Y + direction.Y > desktopSize.Height) && direction.Y > 0)) {  //음수로 가거나 화면크기를 벗어날경우
                return false;
            }
            return true;
        }

        //비슷하니까 구현부는 합쳐도 될듯?


        public override void ChangeDirection() {
            direction.Y = direction.Y * -1; //반전
        }
    }

    public class LeftRightStrategy : MovingStrategy {

        public LeftRightStrategy() {
            direction = new Point(1, 0);
        }

        public override bool CanMove() {
            Point first = formPoint.first;
            Point second = formPoint.second;

            if ((first.X + direction.X < 0 && direction.X < 0) || (second.X + direction.X > desktopSize.Width && direction.X > 0)) {  //음수로 가거나 화면크기를 벗어날경우
                return false;
            }
            return true;
        }

        public override void ChangeDirection() {
            direction.X = direction.X * -1; //반전
        }


    }

    public class BouncingStrategy : MovingStrategy {

        public BouncingStrategy() {

            direction = new Point(1, 1);
        }

        public override bool CanMove() {
            Point first = formPoint.first;
            Point second = formPoint.second;

            if (((first.X + direction.X < 0) && direction.X < 0) || ((second.X + direction.X > desktopSize.Width) && direction.X > 0) || ((first.Y + direction.Y < 0) && direction.Y < 0) || ((second.Y + direction.Y > desktopSize.Height) && direction.Y > 0)) {  //화면크기를 벗어날경우
                return false;
            }
            return true;
        }

        public override void ChangeDirection() {
            Point first = formPoint.first;
            Point second = formPoint.second;
            if ((first.X + direction.X < 0 && direction.X < 0) || (second.X + direction.X > desktopSize.Width && direction.X > 0)) {
                direction.X = direction.X * -1;
            }
            if ((first.Y + direction.Y < 0 && direction.Y < 0) || (second.Y + direction.Y > desktopSize.Height && direction.Y > 0)) {
                direction.Y = direction.Y * -1;
            }
        }
    }

    public class RandomStrategy : MovingStrategy {

        private int delay;
        private long lastChanged = 0;
        private readonly List<Point> directions = new List<Point>() { new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0), new Point(-1, -1) };
        
        public RandomStrategy(int delay) {
            direction = new Point(1, 1);
            this.delay = delay;
        }

        public override bool CanMove() {
            if (CurrentTimeMillis() - lastChanged >= delay * 1000) {
                return false; //바뀌어야됨
            }
            Point first = formPoint.first;
            Point second = formPoint.second;
            if (((first.X + direction.X < 0) && direction.X < 0) || ((second.X + direction.X > desktopSize.Width) && direction.X > 0) || ((first.Y + direction.Y < 0) && direction.Y < 0) || ((second.Y + direction.Y > desktopSize.Height) && direction.Y > 0)) {  //화면크기를 벗어날경우
                return false;
            }
            return true;
        }

        public override void ChangeDirection() {
            List<Point> cloned = new List<Point>(directions);
            cloned.Remove(direction); //현재 방향 제거;
            direction = GetCorrectRandomPoint(cloned);
            lastChanged = CurrentTimeMillis();
        }

        private Point GetCorrectRandomPoint(List<Point> point) {
            while (point.Count > 0) {
                point.Sort((a, b) => new Random().NextDouble() >= 0.5 ? 1 : -1);
                if (check(point[0]))
                    return point[0];
                else
                    point.RemoveAt(0);
            }
            return new Point(-1, -1);
        }

        private bool check(Point point) {
            Point first = formPoint.first;
            Point second = formPoint.second;
            if (((first.X + point.X < 0) && point.X < 0) || ((second.X + point.X > desktopSize.Width) && point.X > 0) || ((first.Y + point.Y < 0) && point.Y < 0) || ((second.Y + point.Y > desktopSize.Height) && point.Y > 0))   //화면크기를 벗어날경우
                return false;
            
            else
                return true;
        }

        private long CurrentTimeMillis() {
            return (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
