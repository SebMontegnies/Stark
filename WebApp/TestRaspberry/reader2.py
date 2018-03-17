import smbus

I2C_ADDRESS = 0x5b

i2c = smbus.SMBus(1)
t = i2c.read_byte_data(I2C_ADDRESS, 0x27)
print(t)
