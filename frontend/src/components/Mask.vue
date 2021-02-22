<template>
  <div id="mask" :style="`--backgroundColor:${backgroundColor}`">
    <slot> </slot>
  </div>
</template>

<script>
export default {
  name: "Mask",
  props: {
    backgroundColor: {
      type: String,
      default: "rgba(0, 0, 0, 0.6)",
    },
  },
};
</script>

<style>
#mask {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;

  z-index: 4;

  display: flex;
  align-items: center;
  justify-content: center;

  min-height: 100vh;
  min-width: 100vw;
  max-width: 100vw;
  background: var(--backgroundColor);
}
</style>
