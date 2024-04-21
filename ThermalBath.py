import time
from pymeasure.instruments.fluke import Fluke7341
from pymeasure.instruments import list_resources
import re




def set_temp(temp, stable_time):
    fluke = Fluke7341("GPIB::22")
    fluke.set_point = temp
    while abs(conver2Float(fluke.temperature) - conver2Float(fluke.set_point)) > 0.1:
        print(fluke.temperature)
        time.sleep(20)
    time.sleep(stable_time)
    print(fluke.temperature)
    pass


def set_temp_without_sync(temp):
    fluke = Fluke7341("GPIB::22")
    fluke.set_point = temp
    pass

	
def get_temperture():
    fluke = Fluke7341("GPIB::22")
    return fluke.temperature


def conver2Float(string):
    match = re.search(r'\d+\.\d+', string)
    if match:
        number = float(match.group())
        return number

# print(list_resources())

if __name__ == "__main__":
    print(list_resources())
    # set_temp(28, 10)
