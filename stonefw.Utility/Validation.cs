using System;

namespace Stonefw.Utility
{
    public static class Validation
    {
        public static ValidationHelper<T> InitValidation<T>(this T value, string name)
        {
            return new ValidationHelper<T>(value, name);
        }

        public static ValidationHelper<string> NotEmpty(this ValidationHelper<string> current)
        {
            current.NotDefault();
            if (string.IsNullOrEmpty(current.Value))
                throw new ArgumentException(String.Format("{0}不能为空！", current.Name));
            return current;
        }

        public static ValidationHelper<string> LongerThan(this ValidationHelper<string> current, int length)
        {
            current.NotDefault();
            if (current.Value.Length < length)
                throw new ArgumentException(String.Format("{0}的长度不可小于{1}！", current.Name, length));
            return current;
        }

        public static ValidationHelper<string> ShorterThan(this ValidationHelper<string> current, int length)
        {
            current.NotDefault();
            if (current.Value.Length > length)
                throw new ArgumentException(String.Format("{0}的长度不可超过{1}！", current.Name, length));
            return current;
        }

        public static ValidationHelper<string> LengthBetween(this ValidationHelper<string> current, int minLength,
            int maxLength)
        {
            current.NotDefault();
            if (current.Value.Length < minLength || current.Value.Length > maxLength)
                throw new ArgumentException(String.Format("{0}的长度必须在{1}和{2}之间！", current.Name, minLength, maxLength));
            return current;
        }

        public static ValidationHelper<int> IsNum(this ValidationHelper<string> current)
        {
            int result;
            if (!int.TryParse(current.Value, out result))
                throw new ArgumentException(String.Format("{0}必须为数字！", current.Name));

            return new ValidationHelper<int>(result, current.Name);
        }

        public static ValidationHelper<int> LargerThan(this ValidationHelper<int> current, int num)
        {
            if (current.Value < num)
                throw new ArgumentException(String.Format("{0}的值不可小于{1}！", current.Name, num));
            return current;
        }

        public static ValidationHelper<int> SmallerThan(this ValidationHelper<int> current, int num)
        {
            if (current.Value > num)
                throw new ArgumentException(String.Format("{0}的值不可大于{1}！", current.Name, num));
            return current;
        }
    }
}