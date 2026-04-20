from datetime import datetime
#1

def count_lines(source):
    with open(source, "r") as f:
        print(len(f.readlines()))

#2

def reverse_file(source, reverse):
    with open(source, "rb") as src:
        l = src.readlines()
        rev_l = l[::-1]
        with open(reverse, "wb") as rev:
                rev.writelines(rev_l)

#3

def logger(exception):
    with open("log.txt", "w") as log:
        log.write(f"ERRROR[{datetime.now()}]:{exception}")


#1
count_lines("input.txt")
#2
reverse_file("input.txt", "reverse.txt")
#3
try:
    error = 2 / 0
except Exception as e: logger(e)



