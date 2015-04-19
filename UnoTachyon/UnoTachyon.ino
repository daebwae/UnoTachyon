
//-----------------------------------------------------------------------------
// MurmurHash3 was written by Austin Appleby, and is placed in the public
// domain. The author hereby disclaims copyright to this source code.

// Note - The x86 and x64 versions do _not_ produce the same results, as the
// algorithms are optimized for their respective platforms. You can still
// compile and run any of them on any platform, but your performance with the
// non-native version will be less than optimal.

//-----------------------------------------------------------------------------
// Platform-specific functions and macros

// Microsoft Visual Studio


#include "MurmurHash2.h"
#include <stdint.h>

#define BIN_BITS 8
#define BINS (1 << BIN_BITS)
#define BIN_BITMASK (BINS - 1)
#define ALPHA (0.71827259325 * BINS * BINS)
#define HLL_HASH_SEED 313

// storage
uint8_t bins[BINS];

int CountLeadingZeros(uint32_t hash, uint8_t maxCount)
{
	uint32_t seeker = 1;
	int counter = 0;
	while (counter < maxCount && (hash & seeker) == 0)
	{
		seeker <<= 1;
		counter++;
	}
	return counter;
}

// Gets the bin index i.e. the bindex :) 
uint8_t GetBinIndex(uint64_t hash)
{
	return hash & BIN_BITMASK;
}

void add(String newInput)
{
	const char* input = newInput.c_str();

	uint64_t hash = MurmurHash64A(input, newInput.length(), HLL_HASH_SEED);

	uint8_t bindex = GetBinIndex(hash);
	uint8_t firstOne = CountLeadingZeros(hash >> BIN_BITS, 64 - BIN_BITS) + 1;

	bins[bindex] = max(bins[bindex], firstOne);

}

double estimate()
{
	double harmonicMean = 0.0;

	for (int i = 0; i < BINS; ++i)
	{
		harmonicMean += 1.0 / pow(2.0, bins[i]);
	}

	return ALPHA / harmonicMean;
}

void setup()
{
	Serial.begin(115200);
}

void loop()
{
	String read = Serial.readStringUntil('\n');

	if (read.length() == 0)
	{
		return;
	}

	add(read);
	Serial.print(estimate());
	Serial.print('\n');
}