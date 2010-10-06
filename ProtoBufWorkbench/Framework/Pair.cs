namespace ProtoBufWorkbench.Framework
{
    /// <summary>
    /// A pair of objects held together.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first object.</typeparam>
    /// <typeparam name="TSecond">The type of the second object.</typeparam>
    public class Pair<TFirst,TSecond>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T, TSecond&gt;"/> class.
        /// </summary>
        /// <param name="first">The first object.</param>
        /// <param name="second">The second object.</param>
        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        /// Gets or sets the first component of the pair.
        /// </summary>
        /// <value>The first component of the pair.</value>
        public TFirst First { get; set; }

        /// <summary>
        /// Gets or sets the second component of the pair.
        /// </summary>
        /// <value>The second component of the pair.</value>
        public TSecond Second { get; set; }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return First.GetHashCode() ^ Second.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is Pair<TFirst, TSecond>)
            {
                var asPair = (Pair<TFirst, TSecond>)obj;
                return Equals(First, asPair.First) &&
                       Equals(Second, asPair.Second);
            }
            return false;
        }
    }
}