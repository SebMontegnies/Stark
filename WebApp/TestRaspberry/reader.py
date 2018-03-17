import smbus

I2C_ADDRESS = 0x57
FIFO_DATA = 0x05

i2c = smbus.SMBus(1)
bytes = i2c.read_i2c_block_data(I2C_ADDRESS, FIFO_DATA, 4)
ir = bytes[0]<<8 | bytes[1]
red = bytes[2]<<8 | bytes[3]
print(ir, red)
