import math

n = int(input())

sum = 0

mod = 10 ** 9 + 7

for i in range(1 , n+1):
    result = math.factorial(n) / (math.factorial(n - i) * math.factorial(i)) * i
    # if i < n:
    #     result = result * (n - i)
    sum += result

print(sum)
print(sum%mod)