#ifndef CONFIG_H
#define CONFIG_H
#pragma once

struct ProfilerConfig
{
	bool AllowSampling;
	bool ProfileGC;
	bool ProfileTransitions;

	bool Instrument;
	bool ProfileUnmanaged;

	bool Load();
};

#endif
