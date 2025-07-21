from PIL import Image
import os
import sys
import json

# Hardcoded options
sprite_filenames = ["Clock.png", "Essence.png"]
base_path = os.path.abspath(os.path.join(os.path.dirname(__file__), "../../Textures"))
output_texture_filename = "TMP_SpriteTextureAtlas.png"
output_json_filename = "TMP_SpriteTextureAtlas.json"
overwrite = True 

def error(msg):
    print("ERROR:", msg)
    sys.exit(1)

def find_visible_bounds(image):
    pixels = image.load()
    for top in range(16):
        if any(pixels[x, top][3] > 0 for x in range(16)):
            break
    else:
        top = 16

    for bottom in reversed(range(16)):
        if any(pixels[x, bottom][3] > 0 for x in range(16)):
            break
    else:
        bottom = -1

    return top, bottom

def main():
    images = []
    index_map = {}

    for i, fname in enumerate(sprite_filenames):
        path = os.path.join(base_path, fname)
        if not os.path.isfile(path):
            error(f"File not found: {path}")
        img = Image.open(path)
        if img.size != (16, 16):
            error(f"Sprite {fname} is not 16x16 pixels (found {img.size})")
        images.append(img.convert("RGBA"))
        index_map[fname] = i

    atlas_width = 16 * len(images)
    atlas_height = 16
    atlas = Image.new("RGBA", (atlas_width, atlas_height))

    for i, img in enumerate(images):
        top, bottom = find_visible_bounds(img)
        if top > bottom:
            continue
        height = bottom - top + 1
        cropped = img.crop((0, top, 16, bottom + 1))
        dst_y = 16 - height
        atlas.paste(cropped, (i * 16, dst_y))

    if os.path.exists(output_texture_filename) and not overwrite:
        error(f"Output file {output_texture_filename} already exists and overwrite is disabled.")

    atlas.save(output_texture_filename)
    print(f"Sprite texture atlas saved to {output_texture_filename}")

    with open(output_json_filename, "w") as f:
        json.dump({
            "keys": list(index_map.keys()),
            "values": list(index_map.values())
        }, f, indent=4)


    print(f"Atlas index JSON saved to {output_json_filename}")

if __name__ == "__main__":
    main()
