from PIL import Image

image = Image.open('source.jpg')
 
grayscale_image = image.convert('L') 
grayscale_bytes = grayscale_image.tobytes() 
grayscale_image.save('grayscale.jpg')

# treshold by 127
treshold_image = image.convert('L') 
treshold_image = treshold_image.point(lambda x: 0 if x < 127 else 255, '1')
treshold_image.save('treshold.jpg')


# export PGM P2 format
with open('PGM_P2.pgm', 'wb') as f:
    f.write(b'P2\n')
    f.write(b'# PGM P2 format\n')
    f.write('{} {}\n'.format(grayscale_image.width, grayscale_image.height).encode())
    f.write(b'255\n')
    for h in range(grayscale_image.height):
        for w in range(grayscale_image.width):
            f.write('{} '.format(grayscale_bytes[h * grayscale_image.width + w]).encode())
        f.write(b'\n')

# export PGM P5 format
with open('PGM_P5.pgm', 'wb') as f:
    f.write(b'P5\n')
    f.write(b'# PGM P5 format\n')
    f.write('{} {}\n'.format(grayscale_image.width, grayscale_image.height).encode())
    f.write(b'255\n')
    f.write(grayscale_bytes)

# export PPM P3 format
with open('PPM_P3.ppm', 'wb') as f:
    f.write(b'P3\n')
    f.write(b'# PPM P3 format\n')
    f.write('{} {}\n'.format(image.width, image.height).encode())
    f.write(b'255\n')
    for h in range(image.height):
        for w in range(image.width):
            f.write('{} {} {} '.format(*image.getpixel((w, h))).encode())
        f.write(b'\n')
    
# export PPM P6 format
with open('PPM_P6.ppm', 'wb') as f:
    f.write(b'P6\n')
    f.write(b'# PPM P6 format\n')
    f.write('{} {}\n'.format(image.width, image.height).encode())
    f.write(b'255\n')
    f.write(image.tobytes())

# export PBM P1 format
with open('PBM_P1.pbm', 'wb') as f:
    f.write(b'P1\n')
    f.write(b'# PBM P1 format\n')
    f.write('{} {}\n'.format(grayscale_image.width, grayscale_image.height).encode())
    for h in range(grayscale_image.height):
        for w in range(grayscale_image.width):
            f.write(b'1 ' if grayscale_bytes[h * grayscale_image.width + w] < 127 else b'0 ')
        f.write(b'\n')

# export PBM P4 format
with open('PBM_P4.pbm', 'wb') as f:
    f.write(b'P4\n')
    f.write(b'# PBM P4 format\n')
    f.write('{} {}\n'.format(grayscale_image.width, grayscale_image.height).encode())
    # byte buffer for 8 pixels (1 byte)
    byte_buffer = 0
    # bit counter
    bit_counter = 0
    for h in range(grayscale_image.height):
        for w in range(grayscale_image.width):
            byte_buffer = byte_buffer << 1
            if grayscale_bytes[h * grayscale_image.width + w] < 127:
                byte_buffer = byte_buffer | 1
            bit_counter = bit_counter + 1
            if bit_counter == 8:
                f.write(bytes([byte_buffer]))
                byte_buffer = 0
                bit_counter = 0
    if bit_counter > 0:
        byte_buffer = byte_buffer << (8 - bit_counter)
        f.write(bytes([byte_buffer]))
