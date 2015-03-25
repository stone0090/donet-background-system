using System;

namespace stonefw.Utility
{
    public class ValidationHelper<T>
    {
        private readonly T _value;
        private readonly string _name;

        /// <summary>    
        /// 获取待验证的参数的值.    
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>    
        /// 获取待验证的参数的名称.    
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>   
        /// 创建一个ValidationHelper的对象.    
        /// </summary>    
        /// <param name="value">待验证的参数的值.</param>    
        /// <param name="name">待验证的参数的名称.</param>
        public ValidationHelper(T value, string name)
        {
            _value = value;
            _name = name;
        }

        /// <summary>    
        /// 验证参数不为其默认值.    
        /// </summary>    
        /// <returns>this指针以方便链式调用.</returns>   
        /// <exception cref="ArgumentException">参数为值类型且为默认值.</exception>    
        /// <exception cref="ArgumentNullException">参数为引用类型且为null.</exception>
        public ValidationHelper<T> NotDefault()
        {
            if (Value.Equals(default(T)))
            {
                if (Value is ValueType)
                {
                    throw new ArgumentException(String.Format("{0}不能为默认值！", Name));
                }
                else
                {
                    throw new ArgumentNullException(String.Format("{0}不能为null值！", Name));
                }
            }

            return this;
        }

        /// <summary>    
        /// 使用自定义方法进行验证.    
        /// </summary>    
        /// <param name="rule">用以验证的自定义方法.</param>    
        /// <returns>this指针以方便链式调用.</returns>    
        /// <exception cref="Exception">验证失败抛出相应异常.</exception>    
        /// <remarks>rule的第一个参数为参数值,第二个参数为参数名称.</remarks>
        public ValidationHelper<T> CustomRule(Action<T, string> rule)
        {
            rule(Value, Name);
            return this;
        }
    }

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

        public static ValidationHelper<string> LengthBetween(this ValidationHelper<string> current, int minLength, int maxLength)
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
