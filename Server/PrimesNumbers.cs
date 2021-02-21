using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Server
{
    public class PrimesNumbers
    {

        public async Task<List<int>> PrimeNumbersFindAsync(int low, int top)
        {
            return await Task.Run((() =>
            {
                var listOfNumbers = new List<int>();
                if (low < 0)
                {
                    low = 0;
                }

                if (top < 0)
                {
                    top = 0;
                }

                for (var i = low; i < top; i++)
                {
                    if (i <= 1)
                    {
                        continue;
                    }

                    var isPrime = true;
                    for (var j = 2; j < i; j++)
                    {
                        if (i % j == 0)
                        {
                            isPrime = false;
                            break;
                        }
                    }

                    if (!isPrime)
                    {
                        continue;
                    }

                    listOfNumbers.Add(i);
                }

                return listOfNumbers;
            }));
            
        }

        public async Task<HttpStatusCode> OneNumberPrimeAsync(int number)
        {
            return await Task.Run((() =>
            {
                if (number <= 1)
                {
                    return HttpStatusCode.BadRequest;
                }

                var isPrime = true;
                for (var j = 2; j < number; j++)
                {
                    if (number % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (!isPrime)
                {
                    return HttpStatusCode.BadRequest;
                }

                return HttpStatusCode.OK;
            }));


        }
    }
}